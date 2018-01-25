Add-Migration "Migration_name" -connectionstringname DefaultConnection

update-database -StartUpProjectName "RaterPrice.Persistence" -connectionstringname "DefaultConnection" -verbose