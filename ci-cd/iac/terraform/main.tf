#############################################################################
# TERRAFORM CONFIG
#############################################################################
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.65"
    }
  }
  required_version = ">= 0.14.9"
}
##################################################################################
# Variables
##################################################################################

variable "default_location" {
  type    = string
  default = "westus2"
}

variable "application_name" {
  type    = string
  default = "qvacar"
}

variable "environment_name" {
  type    = string
  default = "Development"
}

variable "images_container_name" {
  type    = string
  default = "car-ads"
}

variable "cosmos_database_name" {
  type    = string
  default = "QvaCar"
}

variable "sql_server_admin_user" {
  type    = string
  default = "qvaCarAdmin"
}

variable "sql_server_admin_password" {
  type = string
}



##################################################################################
# Locals
##################################################################################

locals {
  environments = {
    Development = "dev"
    Production  = "prod"
  }

  rnd_number = "001"
}

locals {
  environment_prefix = lookup(local.environments, var.environment_name, local.environments["Development"])
}



##################################################################################
# PROVIDERS
##################################################################################
provider "azurerm" {
  features {}
}

##################################################################################
# RESOURCES
##################################################################################

#**Resource Group
resource "azurerm_resource_group" "rgServices" {
  name     = "rg-${var.application_name}-${local.environment_prefix}-${local.rnd_number}"
  location = var.default_location
}

#**Storage Account
resource "azurerm_storage_account" "stAccount" {
  name                     = "st${var.application_name}${local.environment_prefix}${local.rnd_number}"
  resource_group_name      = azurerm_resource_group.rgServices.name
  location                 = azurerm_resource_group.rgServices.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  allow_blob_public_access = true
}

resource "azurerm_storage_container" "stContainer" {
  name                  = var.images_container_name
  storage_account_name  = azurerm_storage_account.stAccount.name
  container_access_type = "blob"
}

#**Cosmos DB
resource "azurerm_cosmosdb_account" "dbCarAds" {
  name                = "cosmos-${var.application_name}-${local.environment_prefix}-${local.rnd_number}"
  location            = azurerm_resource_group.rgServices.location
  resource_group_name = azurerm_resource_group.rgServices.name
  offer_type          = "Standard"
  kind                = "GlobalDocumentDB"

  enable_automatic_failover = false
  enable_free_tier          = true

  consistency_policy {
    consistency_level = "Session"
  }

  geo_location {
    location          = azurerm_resource_group.rgServices.location
    failover_priority = 0
  }
}

resource "azurerm_cosmosdb_sql_database" "dbCarAdsDb" {
  name                = var.cosmos_database_name
  resource_group_name = azurerm_cosmosdb_account.dbCarAds.resource_group_name
  account_name        = azurerm_cosmosdb_account.dbCarAds.name
  throughput          = 400
}

#**Sql Server
resource "azurerm_sql_server" "qvaCarSqlServer" {
  name                         = "sql-${var.application_name}-${local.environment_prefix}-${local.rnd_number}"
  resource_group_name          = azurerm_resource_group.rgServices.name
  location                     = azurerm_resource_group.rgServices.location
  version                      = "12.0"
  administrator_login          = var.sql_server_admin_user
  administrator_login_password = var.sql_server_admin_password
}

resource "azurerm_mssql_database" "sql_qva_car_db" {
  name      = "sqldb-${var.application_name}"
  server_id = azurerm_sql_server.qvaCarSqlServer.id
  collation = "SQL_Latin1_General_CP1_CI_AS"

  max_size_gb                 = 2  
  read_replica_count          = 0
  read_scale                  = false
  sku_name                    = "Basic"
  zone_redundant              = false
}

resource "azurerm_sql_firewall_rule" "insideAzureFirewallRule" {
  name                = "InteralAzureAccessRule"
  resource_group_name = azurerm_resource_group.rgServices.name
  server_name         = azurerm_sql_server.qvaCarSqlServer.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "255.255.255.255"
}

#**App Service
resource "azurerm_app_service_plan" "qvaCarAppPlan" {
  name                = "plan-${var.application_name}-${local.environment_prefix}-${local.rnd_number}"
  location            = azurerm_resource_group.rgServices.location
  resource_group_name = azurerm_resource_group.rgServices.name
  kind                = "linux"
  reserved            = true
  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_app_service" "qvaCarApp" {
  name                = "app-${var.application_name}-${local.environment_prefix}-${local.rnd_number}"
  location            = azurerm_resource_group.rgServices.location
  resource_group_name = azurerm_resource_group.rgServices.name
  app_service_plan_id = azurerm_app_service_plan.qvaCarAppPlan.id

  site_config {
    linux_fx_version   = "DOTNETCORE|6.0"
    scm_type           = "LocalGit"
    websockets_enabled = true
  }
}

#**Application Insights
resource "azurerm_application_insights" "aInsights" {
  name                = "ai-qvacar-dev-001"
  location            = azurerm_resource_group.rgServices.location
  resource_group_name = azurerm_resource_group.rgServices.name
  application_type    = "web"
}

#**Container Instances
resource "azurerm_container_group" "smtpServer" {
  name                = "aci-qvaCar-smptserver-dev-001"
  location            = azurerm_resource_group.rgServices.location
  resource_group_name = azurerm_resource_group.rgServices.name
  ip_address_type     = "public"
  dns_name_label      = "qvaCar-smptserver-dev"
  os_type             = "Linux"

  container {
    name   = "smtp-server"
    image  = "mailhog/mailhog:latest"
    cpu    = "0.5"
    memory = "1.0"

    ports {
      port     = "1025"
      protocol = "TCP"
    }

    ports {
      port     = "8025"
      protocol = "TCP"
    }
  }
}

resource "azurerm_container_group" "elasticSearchServer" {
  name                = "aci-qvaCar-elastic-dev-001"
  location            = azurerm_resource_group.rgServices.location
  resource_group_name = azurerm_resource_group.rgServices.name
  ip_address_type     = "public"
  dns_name_label      = "qvaCar-elastic-dev"
  os_type             = "Linux"

  container {
    name   = "elastic-server"
    image  = "josecdom94/es-aci-dev:7.9.2"
    cpu    = "0.5"
    memory = "1.0"
    environment_variables = {     
      ES_JAVA_OPTS     = "-Xms512m -Xmx512m"
    }

    ports {
      port     = "9200"
      protocol = "TCP"
    }
  }
}

##################################################################################
# OUTPUT
##################################################################################

#**Storage Account
output "storage_connection_string" {
  value     = azurerm_storage_account.stAccount.primary_connection_string
  sensitive = true
}

output "storage_primary_blob_endpoint" {
  value     = azurerm_storage_account.stAccount.primary_blob_endpoint
  sensitive = true
}

output "storage_images_container_name" {
  value     = azurerm_storage_container.stContainer.name
  sensitive = true
}


#**Cosmos DB
output "cosmosdb_endpoint" {
  value     = azurerm_cosmosdb_account.dbCarAds.endpoint
  sensitive = true
}

output "cosmosdb_key" {
  value     = azurerm_cosmosdb_account.dbCarAds.primary_key
  sensitive = true
}

output "cosmosdb_db_name" {
  value = azurerm_cosmosdb_sql_database.dbCarAdsDb.name
}

#**Sql Server
output "sql_server_qva_car_db_connection_string" {
  value = "Server=tcp:${azurerm_sql_server.qvaCarSqlServer.fully_qualified_domain_name};Database=${azurerm_mssql_database.sql_qva_car_db.name};User ID=${var.sql_server_admin_user};Password=${var.sql_server_admin_password};Trusted_Connection=False;Encrypt=True;"
}

#**Application Insights
output "app_insights_instrumentation_key" {
  value     = azurerm_application_insights.aInsights.instrumentation_key
  sensitive = true
}

#**App Service
output "web_app_name" {
  value = azurerm_app_service.qvaCarApp.name
}

output "web_app_hostname" {
  value = "https://${azurerm_app_service.qvaCarApp.default_site_hostname}" 
}

#**Container Instances Services (Temp)
output "container_smtp_host" {
  value = azurerm_container_group.smtpServer.fqdn
}
output "container_elastic_host" {
  value = "http://${azurerm_container_group.elasticSearchServer.fqdn}"
}
