# SearchApplication
Web-Application was implemented with Razor pages. Entity Framework was used to work with the database. 
Code First principle was used to create the database and its elements.

On the Index page you can search info by Google's, Yandex's and Bing's services.
The search is implemented by Search Term.
The response, which comes first, is displayed on the web page and recorded in the database.
If the searchTerm's result is already in the database, these records are deleted and the new results are saved in database.

On the DbSearch page you can search info from the database. This search is also implemented by Search Term.

Client was imlemented For each search's service to send requests and get responoses.
Each client imlements ISearchClient interface.

To start search from Google, Yandex and Bing services you need to set up config file ```appsettings.json```.
You need to enter ConnectionString in ```"DefaultConnection": ""``` field.

In the Google section, you need to specify Url and credentials for search. The following parameters were used for testing: 
```
    "Url": "https://www.googleapis.com/customsearch/v1",
    "Key": "AIzaSyAtmSONG1rljrEgsvcus8gUYmfy5YcWARc",
    "CustomSearchEngineId": "016090189203945702627:n0vlgirvzxf"
```

In the Bing section, you need to specify Url and credentials for search. The following parameters were used for testing:
```
    "Url": "https://api.cognitive.microsoft.com/bing/v7.0/search",
    "Key": "e3a9a702151b4e90a8e3f8d6ba20e00b"
```
  
In the Yandex section, you need to specify Url and credentials for search. The following parameters were used for testing:
```
    "Url": "http://xmlproxy.ru/search/xml",
    "Key": "ifyouneedmorejustpay",
    "User": "test@megaindex.ru"
```
In the case of Yandex service search, we used Xmlproxy service for testing our search. It can provide access to Yandex.XML. 
We used thisy service for tests because after generation key for Yandex.XML service there were some limits of maximum requests per day. The numbers of requests per day were 0.
