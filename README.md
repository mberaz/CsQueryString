this is simple to handle query strings in c#
you can get this as a nuget: https://www.nuget.org/packages/CsQueryString

(if you need to do this in JS, check out the cs version at https://github.com/mberaz/JsQueryString)

to start add the following using to start of the file:
using QueryStringHandler;

you can use it in static mode just like this

var parsed=QueryString.ParseQueryString(url);

if you need to perform actions on the query string, you can create an instance for it

var qs = new QueryString(url);

then you can change the query string values:
qs.InsertOrConcat("con", "first").InsertOrConcat("con", "last");
qs.UpdateValue("updateKey", "updateValue");

qs.DeleteKey("firstname").RenameKey("lastname", "last");
 
returnUrl = qs.RetunrQueryString();


if you need to create a query string form params:
 you can create a Dictionary<string,string> to hold the data and use the following method

 var queryStringWithAQuestionMark = QueryString.CreateQueryString(data);
 
 var queryStringWithOutAQuestionMark = QueryString.CreateQueryString(data,false);
