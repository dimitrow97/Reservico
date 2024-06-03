dotnet tool install --global Evolve.tool

Local
evolve migrate sqlserver -c "Data Source=DESKTOP-DAR1FCK\SQLEXPRESS;Initial Catalog=Reservico;trusted_connection=true" -l .\Migrations\

Dev
evolve migrate sqlserver -c "Server=tcp:reservico-dev.database.windows.net,1433;Initial Catalog=reservico-dev;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;User ID=reservico_admin;Password=B3d0fR0s$eS" -l .\Migrations\
