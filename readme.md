# Bug demo Impersonate and SqlConnection

Demo that `.Impersonate()` doesn't work for SqlConnections.
See: https://github.com/aspnet/Home/issues/1805

## How to run
It's quite an involved process to see this bug.

First - modify the Sql query (the one in the code is specific to my DB = IAMGBLSQLUAT2):
1. Edit the Sql query:
	* $\WebApplicationNet462\Controllers\HomeController.cs
	* See the `About()` Action method
2. Edit the .cshtml which displays the results of the query:
	* see the $\WebApplicationNet462\Views\Home\About.cshtml

Second - run locally = should work (assuming you've got your sql query working)
* NB browse to the /Home/About page to see it
* This should work on your dev machine - i.e. IISEXPRESS is running as you (the logged on user)

Third - publish to a server (with windows authentication enabled)
* NB you need to setup windows authentication in IIS Manager for the site
* now /Home/About will fail because the sql query will be run as the identity that you setup your site as in IIS (e.g. ApplicationPoolIdentity) instead of the windows user that is browsing the website

