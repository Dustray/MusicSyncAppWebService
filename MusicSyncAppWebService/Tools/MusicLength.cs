using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicSyncAppWebService.Tools
{
    public class MusicLength
    {

        /**
         * 获取歌曲长度
         * 
         * @param musicName
         * @return
         */
        //	public static int getMusicLenth(String musicName) {
        //		int sumTime = getAudioPlayTime("D:\\musiclist\\" + musicName + ".mp3");// 获取音乐长度
        //		return sumTime;
        //	}

        public int getAudioPlayTime(String mp3)
        {
            int rtTime = -1;
            //File file = new File(mp3);
            //FileInputStream fis;
            //try
            //{
            //    fis = new FileInputStream(file);
            //    int b = fis.available();
            //    Bitstream bt = new Bitstream(fis);
            //    Header h = bt.readFrame();
            //    int time = (int)h.total_ms(b);
            //    int i = time / 1000;
            //    rtTime = i;
            //    // System.out.println("音乐总长度：" + i / 60 + ":" + i % 60);
            //}
            //catch (Exception e)
            //{
            //    // TODO Auto-generated catch block
            //    e.printStackTrace();
            //}
            return rtTime;
        }

    }
}