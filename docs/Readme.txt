*****DockerCompose*****

**LocalVersion:
docker-compose -f "docker-compose.yaml" -f "docker-compose.override.yaml" -p local-backend up --detach  --build
docker-compose -f "docker-compose.yaml" -f "docker-compose.override.yaml" -p local-backend down

Local + Remove Data:
docker-compose -f "docker-compose.yaml" -f "docker-compose.override.yaml" -p local-backend down --volumes 


*****Commands To Add Migrations for IdenityServer*****
dotnet ef migrations add InitialIdenityMigration --project src\Infraestructure\QvaCar.Infraestructure.Identity  --startup-project src\QvaCar.Host -c QvaCarUsersDBContext -o Migrations/IdentityServer/Identity
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration --project src\Infraestructure\QvaCar.Infraestructure.Identity --startup-project src\QvaCar.Host   -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration --project src\Infraestructure\QvaCar.Infraestructure.Identity --startup-project src\QvaCar.Host -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb