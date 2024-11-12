using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudBrowserPuppeteerClient.Types;
public class AuthorizationException : Exception {

}
public class NoSubscriptionException : Exception {

}
public class NoUnitsException : Exception {

}
public class BrowserLimitException : Exception {

}
public class UnknownException: Exception {

}