using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeeNow.Hubs
{
    //存放連線配對的資料
    public class Player
    {
        /// <summary>
        /// 主要連線
        /// </summary>
        public string ConnectionID { get; set; }

        /// <summary>
        /// 主要的代碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 配對的連線
        /// </summary>
        public string ConnectionID2 { get; set; }

        /// <summary>
        /// 配對的代碼
        /// </summary>
        public string Code2 { get; set; }
    }
}