# Impersonation on SqlConnection with .NET core

Provides a sample app which prints out the username of the current web app user and sql user.

Note you need to use different impersonation code in a .net core app vs .net core app which uses .net framework
see `HomeController.Index`

See https://github.com/aspnet/Home/issues/1805


## How to run
* clone the repo
* change appsettings.json - your DB connection
* build
* setup a site on your webserver with the following:
  * Sites 
    * Authentication
      * Anonymous Authentication = Disabled
      * ASP.NET Impersonation = Enabled
      * Windows Authentication = Enabled
    * Connect as = Application user (pass through)
  * Application Pools
    * .NET CLR version = No Managed Code
    * Pipeline = Classic
* publish to your web server
* browse the site

## How to setup impersonation from webapp to sql

Various out of date links:

### Double hop / Impersonation
* https://blogs.msdn.microsoft.com/chiranth/2014/04/17/setting-up-kerberos-authentication-for-a-website-in-iis/
* http://stackoverflow.com/questions/21891605/getting-iis-to-impersonate-the-windows-user-to-sql-server-in-an-intranet-environ
* http://stackoverflow.com/questions/4618552/iis-to-sql-server-kerberos-auth-issues?rq=1
* http://stackoverflow.com/questions/13706580/kerberos-double-hop-in-asp-net-4-0-sql2008r2?rq=1
* https://shuggill.wordpress.com/2015/03/06/configuring-sql-server-kerberos-for-double-hop-authentication/
* https://social.technet.microsoft.com/wiki/contents/articles/717.service-principal-names-spns-setspn-syntax-setspn-exe.aspx
* https://support.microsoft.com/en-us/kb/973917

* Install Remote Server Administration Tool (RSAT) on server - windows server manager > add roles and features > features > Remote Server Administration Tool
* Must run iis as fixed user account (so you can create spn for iis)
* Must run sqlserver as fixed user account (so you can create spn for sqlserver)
* Configure website - enable impersonation and windows auth, run as fixed user
* Setup SPNs

```
setspn –a iisaccountname HTTP/IAMLINVWEBDEV
setspn –a iisaccountname HTTP/IAMLINVWEBDEV.investecam.corp 

setspn -a sqlaccountname MSSQLSvc/host:instanceName
setspn -a sqlaccountname MSSQLSvc/host:<TCPPORT>
setspn -a sqlaccountname MSSQLSvc/host.domain.com:instanceName
setspn -a sqlaccountname MSSQLSvc/host.domain.com:<TCPPORT>
```
* Run dsa.msc to run RSAT
