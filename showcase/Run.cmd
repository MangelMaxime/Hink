@ECHO OFF
ECHO "Restoring npm packages..."
CALL npm install
ECHO "Restoring dotnet packages..."
CALL dotnet restore
ECHO "Starting fable..."
CALL dotnet fable npm-run start
