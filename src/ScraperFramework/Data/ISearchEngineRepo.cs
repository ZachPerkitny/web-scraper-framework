﻿using System.Collections.Generic;
using ScraperFramework.Data.Entities;

namespace ScraperFramework.Data
{
    public interface ISearchEngineRepo
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngine"></param>
        void Insert(SearchEngine searchEngine);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngines"></param>
        void InsertMany(IEnumerable<SearchEngine> searchEngines);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineId"></param>
        /// <returns></returns>
        SearchEngine Select(short searchEngineId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEngineGroup"></param>
        /// <returns></returns>
        IEnumerable<SearchEngine> SelectMany(int searchEngineGroup);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SearchEngine> SelectAll();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ulong Count();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SearchEngine Max();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        SearchEngine Min();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        byte[] GetLatestRevision();
    }
}