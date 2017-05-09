using MrHai.Common.GlobalConfig;
using MrHai.Core.Models;
using System.Collections.Generic;
using TwinkleStar.Common.Others;

namespace MrHai.Core
{
    public interface IMrHaiService
    {
        #region GlobalConfig部分
        /// <summary>
        /// 获取友情链接
        /// </summary>
        OperationResult GetFriendlyLinks();

        /// <summary>
        /// 保存友情链接
        /// </summary>
        int SaveFriendlyLinks(FriendlyLinks friendlyLinks);

        /// <summary>
        /// 获取关于我们
        /// </summary>
        /// <returns></returns>
        OperationResult GetAboutUs();
        /// <summary>
        /// 保存关于我们
        /// </summary>
        /// <param name="aboutUs"></param>
        void SaveAboutUs(AboutUs aboutUs);

        /// <summary>
        /// 获取碎片配置
        /// </summary>
        /// <returns></returns>
        OperationResult GetFragment();

        /// <summary>
        /// 获取碎片配置
        /// </summary>
        /// <param name="num">碎片编号</param>
        /// <returns></returns>
        OperationResult GetFragment(int num);
        /// <summary>
        /// 保存碎片配置
        /// </summary>
        /// <param name="fragment"></param>
        void SaveFragment(Fragment fragment);

        /// <summary>
        /// 获取logo
        /// </summary>
        /// <returns></returns>
        OperationResult GetLogo();
        /// <summary>
        /// 保存logo
        /// </summary>
        /// <param name="logo"></param>
        void SaveLogo(Logo logo);

        /// <summary>
        /// 获取官网链接
        /// </summary>
        /// <returns></returns>
        OperationResult GetOfficialWebsite();
        /// <summary>
        /// 保存官网链接
        /// </summary>
        /// <param name="officialWebsite"></param>
        void SaveOfficialWebsite(OfficialWebsite officialWebsite);

        /// <summary>
        /// 获取最新作品
        /// </summary>
        /// <returns></returns>
        OperationResult GetNewWorks();
        /// <summary>
        /// 保存最新作品
        /// </summary>
        /// <param name="newWorks"></param>
        void SaveNewWorks(NewWorks newWorks);
        #endregion

        #region Information部分

        int SaveInfomation(List<Infomation> list);


        int UpdateinfomationChanges(Infomation entiy);
        int UpdateinfomationChanges(List<Infomation> list);
        int DeleteInfomation(List<Infomation> list);
        string GetModelByTitle(string title,string workID ,InfomationType type);

        OperationResult GetInfomationByType(InfomationType type);
        OperationResult GetInfomationModelByType(InfomationType type);
        OperationResult GetInfomations(List<string> categoryIds, List<InfomationType> types);
        #endregion

        #region Category部分

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        OperationResult GetCategories(NavigationType type = NavigationType.MainNavigation, List<string> pids = null, bool isGetEnabled = false);
        OperationResult GetCategories(List<string> ids);
        OperationResult GetCategory(string id);
        OperationResult GetRootCategories();
        OperationResult GetChildCategories(string pid, NavigationType type);  
        OperationResult GetWorksName();
        OperationResult GetWorksName(string pId);

        int AddCategory(string name, string pid, NavigationType type, bool isThird = false);
        //新增一个分类，并且返回ID
        string AddCategoryAndReturnId(string name, string pid, NavigationType type);
        int AddChildCategory(string id);
        int EditCategory(string name, string id);
        int SortCategory(string id, int num);
        int DeleteCategory(string id, bool isDelChild = true);
        int ChangeCategoryEnabled(string id);
        #endregion

        #region Works部分
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pid">作品的父级别</param>
        /// <param name="recordCount"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        OperationResult SearchWorks(string name, string pid, ref int recordCount, int pageIndex, int pageCount);
        /// <summary>
        /// 查询作品
        /// </summary>
        /// <param name="name">名称查询</param>
        /// <param name="recordCount">集合总数</param>
        /// <param name="pageIndex">页码 0开始</param>
        /// <param name="pageCount">单页总数</param>
        /// <returns></returns>
        OperationResult SearchWorks(string name, ref int recordCount, int pageIndex, int pageCount);

        OperationResult CommonSearch(string name,string caID, InfomationType infoType, ref int recordCount, int pageIndex, int pageCount);
        #endregion

        #region 资讯部分

        OperationResult GetInfos(string id, NodeType type);
        OperationResult GetInfo(string id);
        OperationResult GetInfoByPid(string pid);
        OperationResult GetInfoByPid(string pid, InfomationType type);
        OperationResult GetInfo(InfomationType type);
        //OperationResult GetInfo(InfomationType type,string categoryId);
        #endregion

        #region Attachment部分
        int GetMaxAttachmentID();
        string GetModelCode(Attachment model);

        int InsertAttachment(Attachment model);

        #endregion

        #region User部分
        OperationResult Login(string userCode, string password);
        #endregion
    }
}
