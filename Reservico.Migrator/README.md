dotnet tool install --global Evolve.tool

Local
evolve migrate sqlserver -c "Data Source=DESKTOP-DAR1FCK\SQLEXPRESS;Initial Catalog=Reservico;trusted_connection=true" -l .\Migrations\

Dev
evolve migrate sqlserver -c "Server=tcp:reservico-dev.database.windows.net,1433;Initial Catalog=reservico-dev;Persist Security Info=False;User ID=reservico_admin;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" -l .\Migrations\
