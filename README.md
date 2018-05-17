this is a simple to handle query strings in c#

to start add the using QueryStringHandler;

you can use it in static mode just like this

var parsed=QueryString.ParseQueryString(url, DuplicateKeyMode.Concat);

DuplicateKeyMode set the behavior when a key shows more then once in the query string, the options are: Concat, Replase and KeepOld

if you need to perfome actions on the query string, you can create an instanse for it

var qs = new QueryString(url, DuplicateKeyMode.Concat);

then you can change the query string values:
qs.InsertOrConcat("con", "first").InsertOrConcat("con", "last");
qs.UpdateValue("updateKey", "updateValue");
qs.DeleteKey("firstname").RenameKey("lastname", "last");
 
returnUrl = qs.RetunrQueryString();
