using MusicSyncAppWebService.Tools;
using System;
using System.Web.Services;

namespace MusicSyncAppWebService
{
    /// <summary>
    /// MusicSync 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://services.dustray.cn/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MusicSync : System.Web.Services.WebService
    {

        [WebMethod]
        public string getSynTime(String teamName, String musicName)
        {
            //return "::"+ teamName+"++"+musicName;
            string result = "ss";
            try
            {
                DbProcess msd = new DbProcess(teamName);
                // 验证是否已存在某条舞团播放信息
                if (msd.getTeam())
                {// true为已存在
                 // 获取音乐播放状态
                    msd.setSyncIdFromTeamName(teamName);
                    if (msd.getMusicState() == 1)
                    {// 1-播放中；0-暂停；-1-默认状态
                     // 获取音乐开始播放日期时间
                        DateTime ts = msd.getStartDateTime();// 开始播放的日期时间
                        long s = StringProcess.ConvertDataTimeLong(DateTime.Now) - StringProcess.ConvertDataTimeLong(ts);

                        return ""+s;//播放进度
                    }
                    else if (msd.getMusicState() == 0)
                    {
                        long s = msd.getPauseTime();
                        return ""+-1;//"已在"+s+"毫秒处暂停";
                    }
                    else
                    {
                        return ""+-2;//"播放状态错误";
                    }
                }
                else
                {
                    // 添加
                    System.Diagnostics.Debug.Write(msd.getTeam());
                    if (msd.addNewMusic(musicName))
                    {
                        return ""+0;//"已添加成功";
                    }
                    return ""+-3;//"添加失败";
                }
            }
            catch (Exception e)
            {
                result =  2017 +e.ToString();
            }
                return result;
            
        }

        /**
         * 歌曲暂停
         * 
         * @param teamName
         * @param musicName
         * @return
         */
        [WebMethod]
        public String pauseMusic(String teamName, String musicName)
        {
            DbProcess msd = new DbProcess(teamName);
            if (msd.getTeam())
            {// true为已存在
             // 获取音乐播放状态
                msd.setSyncIdFromTeamName(teamName);
                return msd.pauseMusic();
            }
            
            else
            {
                return "Team not found";
            }
        }

        /**
         * 歌曲播放
         * 
         * @param teamName
         * @param musicName
         * @return
         */
        [WebMethod]
        public String continueMusic(String teamName, String musicName)
        {
            DbProcess msd = new DbProcess(teamName);
            if (msd.getTeam())
            {// true为已存在
             // 获取音乐播放状态
                msd.setSyncIdFromTeamName(teamName);
                return msd.continueMusic();
            }
            else
            {
                return "Team not found";
            }
            
        }

        /**
         * 获取音乐长度
         * 
         * @param musicName
         * @return
         */
        [WebMethod]
        public int getMusicLength(String teamName, String musicName)
        {
            MusicLength ml = new MusicLength();
            int sumTime = ml.getAudioPlayTime("D:\\musiclist\\" + musicName
                    + ".mp3");// 获取音乐长度
            return sumTime;
        }
    }
}
