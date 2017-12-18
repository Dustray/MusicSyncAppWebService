using MusicSyncAppWebService.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MusicSyncAppWebService.Tools
{

    public class DbProcess
    {
        private MusicSyncModel db = new MusicSyncModel();
        private String teamName = "";
        private int syncId = 0;

        public DbProcess(string teamName)
        {
            this.teamName = teamName;
            //this.syncId = getSyncIdFromTeamName(teamName);
        }
        public int setSyncIdFromTeamName(String teamName)
        {
            int flag = (from n in db.MusicSync
                        where n.teamName == teamName
                        select n).First().syncId ;
            this.syncId = flag;
            return flag;
        }
        /**
         * 添加音乐播放
         * 
         * @param teamName
         * @param musicName
         * @return
         */

        public bool addNewMusic(String musicName)
        {
            bool flag = false;
            //MusicLength ml = new MusicLength();
            //int sumTime = ml.getAudioPlayTime("D:\\musiclist\\" + musicName+".mp3");// 获取音乐长度
            try
            {

                SyncEntity ms = new SyncEntity();
                ms.teamName = teamName;
                ms.musicName = musicName;
                ms.startDateTime = DateTime.Now;
                ms.pauseTime = -1;
                ms.playState = 1;
                db.MusicSync.Add(ms);
                db.SaveChanges();
                flag = true;

                setSyncIdFromTeamName(teamName);
            }
            catch (Exception e)
            {

                System.Diagnostics.Debug.Write(e.ToString());
            }
            return flag;
        }

        /**
         * 验证是否已存在某条舞团播放信息
         * 
         * @param teamName
         * @return
         */
        public bool getTeam()
        {
            bool flag = false;
            //setSyncIdFromTeamName(teamName);
            try
            {
                int countNum = (from n in db.MusicSync
                                where n.teamName == teamName
                                select n).Count();
                // System.Diagnostics.Debug.Write(countNum);
                if (countNum == 1)
                {
                    flag = true;
                }
                else if (countNum > 1)
                {
                    // 执行删除操作
                    if (deleteTeam())
                    {
                        System.Diagnostics.Debug.Write("删除成功");
                    }
                    else
                    {
                        System.Diagnostics.Debug.Write("删除失败");
                    }
                }

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write("获取列表失败：" + e.ToString());
            }
            return flag;
        }

        /**
         * 获取音乐播放状态
         * 
         * @param teamName
         * @return
         */
        public int getMusicState()
        {
            int flag = -1;
            try
            {

                flag = (from n in db.MusicSync
                        where n.syncId == syncId
                        select n).First().playState;

            }
            catch (Exception e)
            {

            }
            return flag;
        }
        /**
         * 获取音乐名称
         * 
         * @param teamName
         * @return
         */
        public String getMusicName()
        {
            String flag = "null";
            try
            {
                flag = (from n in db.MusicSync
                        where n.syncId == syncId
                        select n).First().musicName;
            }
            catch (Exception e)
            {

            }
            return flag;
        }
        /**
         * 删除所有这个舞队信息
         * 
         * @param teamName
         * @return
         */
        public bool deleteTeam()
        {
            bool flag = false;
            try
            {

                SyncEntity ms = new SyncEntity();
                ms.teamName = teamName;
                db.MusicSync.Attach(ms);
                db.MusicSync.Remove(ms);
                db.SaveChanges();
                flag = true;
            }
            catch (Exception e)
            {

            }
            return flag;
        }

        /**
         * 获取音乐开始播放日期时间
         * 
         * @param teamName
         * @return
         */
        public DateTime getStartDateTime()
        {
            DateTime flag = DateTime.Now;
            try
            {
                flag = (from n in db.MusicSync
                                 where n.syncId == syncId
                                 select n).First().startDateTime;
            }
            catch (Exception e)
            {

            }
            return flag;
        }

        /**
         * 获取暂停时已播放进度
         * 
         * @param teamName
         * @return
         */
        public long getPauseTime()
        {
            long flag = -1;
            try
            {
                flag = (from n in db.MusicSync
                        where n.syncId == syncId
                        select n).First().pauseTime;

            }
            catch (Exception e)
            {

            }
            return flag;
        }

        /**
         * 音乐暂停（修改播放状态为0并记录已播放长度）
         * 
         * @param teamName
         * @return
         */

        public String pauseMusic()
        {
            String flag = "初始flag";
            long sTime = -1;
            if (getMusicState() == 0)
            {
                flag = "当前已是暂停状态";
            }
            else
            {
                if (getStartDateTime() == null)
                {
                    flag = "未找到歌曲时间";
                }
                else
                {
                    sTime = StringProcess.ConvertDataTimeLong(DateTime.Now)- StringProcess.ConvertDataTimeLong(getStartDateTime());
                }
                try
                {
                    SyncEntity ms = db.MusicSync.Find(syncId);
                    ms.syncId = syncId;
                    ms.pauseTime = sTime;
                    ms.playState = 0;
                    db.Entry(ms).State = EntityState.Modified;
                    db.SaveChanges();
                    flag = "暂停成功";
                }
                catch (Exception e)
                {
                    flag = "暂停失败：" + e.ToString();
                }
            }
            return flag;

        }

        /**
         * 音乐继续（修改播放状态为1并记录继续的瞬时时间-已播放长度）
         * 
         * @param teamName
         * @return
         */
        public String continueMusic()
        {
            String flag = "初始flag";
            long sTime = -1;
            if (getMusicState() == 1)
            {
                flag = "当前已是播放状态";
            }
            else
            {
                if (getPauseTime() == -1)
                {
                    flag = "未找到已播放时间";
                }
                else
                {
                    sTime = getPauseTime();
                    // 修改播放状态为1并记录继续的瞬时时间-已播放长度
                    long t =StringProcess.ConvertDataTimeLong(DateTime.Now) - sTime;
                    DateTime ts = StringProcess.ConvertLongDateTime(t);
                    try
                    {
                        
                        SyncEntity ms = db.MusicSync.Find(syncId);
                        ms.syncId = syncId;
                        ms.startDateTime = ts;
                        ms.pauseTime = -1;
                        ms.playState = 1;
                        db.Entry(ms).State = EntityState.Modified;
                        db.SaveChanges();
                        flag = "继续成功";
                    }
                    catch (Exception e)
                    {
                        flag = "继续失败："+e.ToString();
                    }

                }
            }
            return flag;

        }
    }

}