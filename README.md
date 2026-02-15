-- Add migration with the changes

dotnet ef migrations add AddFuelAlert --project src/VTS.DAL/VTS.DAL.csproj --startup-project src/VTS.API/VTS.API.csproj

-- Apply migration with the changes

dotnet ef database update \
 --project VTS.DAL \
 --startup-project VTS.API \
 --context VTSContext
