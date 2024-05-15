dotnet tool install --global Evolve.tool

Local
evolve migrate sqlserver -c "Data Source=DESKTOP-DAR1FCK\SQLEXPRESS;Initial Catalog=Reservico;trusted_connection=true" -l .\Migrations\