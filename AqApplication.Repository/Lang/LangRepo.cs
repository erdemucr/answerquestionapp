using AnswerQuestionApp.Entity.Lang;
using AqApplication.Entity.Identity.Data;
using AqApplication.Repository.Enums;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnswerQuestionApp.Repository.Lang
{
    public class LangRepo : ILang
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private IMemoryCache _cache;
        private bool disposedValue = false;
        public LangRepo(ApplicationDbContext _context, IMemoryCache memoryCache)
        {
            context = _context;
            _cache = memoryCache;
        }
        public List<LangContent> LangValues()
        {
            var cacheEntry = _cache.Get<List<LangContent>>(CacheKeys.LangContent);
            if (cacheEntry == null)
            {
                var list = context.LangContent.ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(180));

                _cache.Set(CacheKeys.LangContent, list, cacheEntryOptions);

                return list;
            }
            return cacheEntry;
        }
        public string GetLangValue( L langKey, string lang)
        {
            if (lang == "tr-TR")
                return LangValues().First(x => x.Key == langKey).TrValue;
            return LangValues().First(x => x.Key == langKey).EngValue;
        }
    }
}
