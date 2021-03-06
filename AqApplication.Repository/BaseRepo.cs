﻿using AnswerQuestionApp.Entity.Configuration;
using AqApplication.Entity.Identity.Data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AqApplication.Repository.Enums;

namespace AnswerQuestionApp.Repository
{
    public class BaseRepo
    {
        private IMemoryCache _cache;
        ApplicationDbContext context;
        public BaseRepo(ApplicationDbContext _context, IMemoryCache memoryCache)
        {
            _cache = memoryCache;
            context = _context;
        }

        public List<ConfigurationValues> ConfigurationValues
        {
            get
            {
                var cacheEntry = _cache.Get<List<ConfigurationValues>>(CacheKeys.ConfigurationValues);
                if (cacheEntry == null)
                {
                    var list = context.ConfigurationValues.ToList();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(180));

                    _cache.Set(CacheKeys.ConfigurationValues, list, cacheEntryOptions);

                    return list;
                }
                return cacheEntry;
            }
        }
        public int challengeExpDay
        {
            get
            {
                var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeTimeDuration);
                if (cValue != null)
                {
                    return Convert.ToInt32(cValue.Values);
                }
                return -1;
            }
        }

        public int challengeDuration
        {
            get
            {
                var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeTimeDuration);
                if (cValue != null)
                {
                    return Convert.ToInt32(cValue.Values);
                }
                return -1;
            }
        }

        public int challengeQuestionLimit
        {
            get
            {
                var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeQuestionCount);
                if (cValue != null)
                {
                    return Convert.ToInt32(cValue.Values);
                }
                return -1;
            }
        }
        public int challengeQuestionCount
        {
            get
            {
                var cValue = ConfigurationValues.FirstOrDefault(x => x.Key == ConfigKey.ChallengeQuestionCount);
                if (cValue != null)
                {
                    return Convert.ToInt32(cValue.Values);
                }
                return -1;
            }
        }
  
    }
}
