﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace EpgTimer
{
    public static class EpgAutoAddDataEx
    {
        public static uint SearchCount(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).SearchCount;
        }
        public static uint ReserveCount(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).ReserveCount;
        }
        public static uint OnCount(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).OnCount;
        }
        public static uint OffCount(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).OffCount;
        }
        public static List<SearchItem> GetSearchList(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).SearchItemList;
        }
        public static List<ReserveData> GetReserveList(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).ReseveItemList;
        }
        public static ReserveData GetNextReserve(this EpgAutoAddData master)
        {
            return CommonManager.Instance.DB.GetEpgAutoAddDataAppend(master).NextReserve;
        }
    }

    public class EpgAutoAddDataAppend
    {
        public EpgAutoAddDataAppend(EpgAutoAddData master1, List<EpgEventInfo> eventlist = null)
        {
            master = master1;
            epgEventList = eventlist;
        }

        private EpgAutoAddData master = null;
        private List<EpgEventInfo> epgEventList = null;
        private List<SearchItem> searchItemList = null;
        private List<ReserveData> reseveItemList = null;
        private ReserveData nextReserve = null;
        private uint searchCount = 0;
        private uint onCount = 0;
        private uint offCount = 0;

        //予約情報の更新があったとき、CommonManager.Instance.DB.epgAutoAddAppendList()に入っていればフラグを立ててもらえる。
        public bool updateCounts = true;

        public EpgAutoAddData Master            { get { return master; } }
        public uint dataID                      { get { return (master != null ? master.dataID : 0); } }
        public uint SearchCount                 { get { RefreshData(); return searchCount; } }
        public uint ReserveCount                { get { RefreshData(); return onCount + offCount; } }
        public uint OnCount                     { get { RefreshData(); return onCount; } }
        public uint OffCount                    { get { RefreshData(); return offCount; } }
        public List<SearchItem> SearchItemList  { get { RefreshData(); return searchItemList; } }
        public List<ReserveData> ReseveItemList { get { RefreshData(); return reseveItemList; } }
        public ReserveData NextReserve          { get { RefreshData(); return nextReserve; } }

        public List<EpgEventInfo> EpgEventList
        {
            get
            {
                if (epgEventList == null) this.EpgEventList = new List<EpgEventInfo>();
                return epgEventList;
            }
            set
            {
                epgEventList = value;
                updateCounts = true;
            }
        }

        //必要なら情報の更新をする。
        public void RefreshData()
        {
            if (updateCounts == false) return;
            updateCounts = false;

            searchCount = 0;
            onCount = 0;
            offCount = 0;

            if (master == null || epgEventList == null) return;

            searchItemList = new List<SearchItem>();
            searchItemList.AddFromEventList(epgEventList, false, true);

            reseveItemList = searchItemList.GetReserveList();

            searchCount = (uint)searchItemList.Count;
            onCount = (uint)reseveItemList.Count(info => info.RecSetting.RecMode != 5);
            offCount = (uint)reseveItemList.Count - onCount;
            nextReserve = reseveItemList.GetNextReserve();
        }

    }
}