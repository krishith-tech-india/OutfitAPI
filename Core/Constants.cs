using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public class Constants
{
    public const string NotExistExceptionMessage = "{0} with {1} {2} does not exist";
    public const string FieldrequiredExceptionMessage = "{0}, {1} is required";
    public const string AleadyExistExceptionMessage = "{0} {1} {2} is aleady exist";
    public const string DependentFindExceptionMessage = "Record Can't Delete because There Are Dependent fileds on {0}";
    public const string UnauthorizedExceptionMessage = "You Are Not Allowed To Perform This Action";
    public const string TokenMissingExceptionMessage = "Token is Missing OR invalid.";
}
