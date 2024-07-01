# FamilyTree
## Application launch
### In Docker
In the folder with the cloned project, run command:
```cmd
docker-compose -f docker-compose.yml build
```
And then command:
```cmd
docker-compose -f docker-compose.yml up -d
```
Go to http://localhost:8080/swagger/index.html to check API. Note: If the page is not accessible, try stopping the created Docker container and running the second command again.
### In IDE
In appsettings.json file change the DefaultConnection value to your Postgres server's connection string.
```json
"DefaultConnection": "Host=database;Database=FamilyTree;Username=postgres;Password=postgres"
```
You can choose the database value at your discretion - it will be created with the name you set here. Next, launch the application using your IDE.
