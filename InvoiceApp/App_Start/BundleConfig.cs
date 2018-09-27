/**************************************************************************************************************************************************
'Created By     : Ventech Team
'Created Date   :  
'Description    : All script bundles
'
'
'Change History:
'------------------------------------------------------------------------------------------
'Changed By                 Changed Date            Description
'------------------------------------------------------------------------------------------
'
'
'  
'****************************************************************************************************************************************************/
using System.Web.Optimization;

namespace InvoiceApp
{
    public class BundleConfig
    {

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {




            BundleTable.EnableOptimizations = false;
        }
    }
}