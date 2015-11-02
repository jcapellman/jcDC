﻿using jcDC.Library.Enums;
using jcDC.Library.Objects;
using jcDC.Library.EFModel;

using System.Linq;

using Newtonsoft.Json;

namespace jcDC.Library.CachingPlatforms {
    public class SQLCachePlatform : BaseCachePlatform {
        public override void AddToCache<T>(string key, T value) {
            using (var eFactory = new jcDCEFModel()) {
                var isNew = true;

                var cacheItem = eFactory.Caches.FirstOrDefault(a => a.Key == key);

                if (cacheItem == null) {
                    cacheItem = eFactory.Caches.Create();

                    cacheItem.Key = key;
                } else {
                    isNew = false;
                }

                cacheItem.KeyValue = JsonConvert.SerializeObject(value);

                if (isNew) {
                    eFactory.Caches.Add(cacheItem);
                }

                eFactory.SaveChanges();
            }        
        }

        public override CACHINGPLATFORMS GetCachingPlatformType() {
            return CACHINGPLATFORMS.SQL;
        }

        public override jcCACHEItem GetFromCache(string key) {
            using (var eFactory = new jcDCEFModel()) {
                var cacheItem = eFactory.Caches.FirstOrDefault(a => a.Key == key);

                if (cacheItem == null) {
                    return null;
                }

                return JsonConvert.DeserializeObject<jcCACHEItem>(cacheItem.KeyValue);
            }
        }

        public override void RemoveDependencies(string[] dependencies) {
            using (var eFactory = new jcDCEFModel()) {
                foreach (var item in dependencies) {
                    eFactory.Database.ExecuteSqlCommand($"DELETE FROM dbo.Cache WHERE [Key] = '{item}'");
                }
            }
        }
    }
}