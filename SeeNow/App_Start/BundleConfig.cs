using System.Web;
using System.Web.Optimization;

namespace SeeNow
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Meta 後台用
            bundles.Add(new StyleBundle("~/back/css").Include(
                      "~/Content/bootstrap.css"));
            bundles.Add(new StyleBundle("~/layui/css").Include(
                      "~/Scripts/layui/css/layui.css"));
            bundles.Add(new ScriptBundle("~/layui/js").Include(
                      "~/Scripts/layui/layui.js"));

            //Legai 前台用
            bundles.Add(new StyleBundle("~/magnific-popup/css").Include(
                      "~/Content/magnific-popup/magnific-popup.css"));

            //字形
            bundles.Add(new StyleBundle("~/fontawesome-free/css").Include(
                     "~/Content/fontawesome-free/all.min.css"));

            //Legai 前台用
            bundles.Add(new StyleBundle("~/freelancer/css").Include(
                     "~/Content/freelancers/freelancer.min.css"));

            


        }
    }
}