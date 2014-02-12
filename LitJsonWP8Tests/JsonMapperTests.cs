using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Collections.Generic;
using System.Threading;

namespace LitJsonWP8Tests
{
    [TestClass]
    public class JsonMapperTests
    {

        //[Serializable]
        public class cPlayerRecord
        {
            public int playerId;
            public bool newPlayerRecord;
            public bool newWispModeMessage; // To ignore... Kept here to ensure save game's integrity
            public bool showedSecondChanceMessage;
            public bool showedNewCoinPackNotification;
            public int bestScore;
            public float bestScoreMultiplierFromSecondChance;
            public int bestCoinScore;
            public int bestDistanceScore;
            public int lifetimePlays;
            public int lifetimeCoins;
            public int lifetimeDistance;
            public int coinCount;
            public int mostTargets;
            public int wispCount;
            public int lifetimeWispCount;

            public int lifetimePointsWonInSecondChance;
            public int fergusLifetimeAxesThrown;
            public int fergusLifetimeBellyFlops;
            public int fergusLifetimeSecondChanceTotal;
            public int fergusLifetimeDistance;
            public int fergusLifetimeTimesInWispWorld;

            public int scoreMultiplier;	// DEPRECATED - Use RecordManager.Instance.GetScoreMultitplier instead (yes, there's a typo)
            public int activePlayerCharacter;
            public int[] storeItemLevel;
            public int[] flags;
            public bool gameCenterNeedsUpdate;

            //// JLOUCHEZ : Not sure how changing the type would affect serialization backward compatibility, so I'll just cast for now.
            //public CharacterId GetActiveCharacterId()
            //{
            //    return (CharacterId)activePlayerCharacter;
            //}

            //public void SetActiveCharacterId(CharacterId aId)
            //{
            //    activePlayerCharacter = (int)aId;
            //}

            //public cPlayerRecord()
            //    : this(0)
            //{
            //}

            //public cPlayerRecord(int newPlayerId)
            //{

            //    storeItemLevel = new int[(int)StoreItemType.kStoreItemCount];
            //    flags = new int[(int)RecordFlagType.kRecordFlagCount];
            //    SetDefaults();
            //    playerId = newPlayerId;
            //}

            //public void SetDefaults()
            //{
            //    flags[(int)RecordFlagType.kRecordFlagVideoAdOfferAvailable] = 1;
            //    flags[(int)RecordFlagType.kRecordFlagVideoAdReward] = 250;
            //    flags[(int)RecordFlagType.kRecordFlagVideoAdPercentFlurry] = 100;
            //    flags[(int)RecordFlagType.kRecordFlagFullScreenAdOfferAvailable] = 1;
            //    flags[(int)RecordFlagType.kRecordFlagFullScreenAdReward] = 5000;
            //    flags[(int)RecordFlagType.kRecordFlagFullScreenAdDisableCost] = 2500;
            //    flags[(int)RecordFlagType.kRecordFlagFullScreenAdPercentFaad] = 50;
            //    flags[(int)RecordFlagType.kRecordFlagTwitterOfferAvailable] = 1;
            //    flags[(int)RecordFlagType.kRecordFlagFullScreenAdIncentivizedPercentage] = 0;
            //    flags[(int)RecordFlagType.kRecordFlagFullScreenAdFrequency] = 100;
            //    newPlayerRecord = true;
            //    showedSecondChanceMessage = false;
            //    showedNewCoinPackNotification = false;
            //    bestScore = bestCoinScore = bestDistanceScore = lifetimePlays = lifetimeCoins = lifetimeDistance = coinCount = wispCount = lifetimeWispCount = 0;
            //    bestScoreMultiplierFromSecondChance = 1f;
            //    lifetimePointsWonInSecondChance = fergusLifetimeAxesThrown = fergusLifetimeBellyFlops = fergusLifetimeDistance = fergusLifetimeSecondChanceTotal = fergusLifetimeTimesInWispWorld = 0;

            //    scoreMultiplier = 0;
            //    SetActiveCharacterId(CharacterId.DEFAULT);
            //    gameCenterNeedsUpdate = true;
            //}

        };
        //[Serializable]
        public class RecordData
        {
            public List<cPlayerRecord> PlayerRecords;
            //public List<cAchievementRecord> AchievementRecords;
            //public List<AchievementEnum> StoreAchievementsRecords;
            //public List<cGameCenterScore> FriendScores;
            public bool RestoreCompletedTransaction;

            public RecordData()
            {
                this.PlayerRecords = new List<cPlayerRecord>();
                //this.AchievementRecords = new List<cAchievementRecord>();
                //this.StoreAchievementsRecords = new List<AchievementEnum>();
                //this.FriendScores = new List<cGameCenterScore>();
                this.RestoreCompletedTransaction = true;
            }
        }

        [TestMethod]
        public void SerializeRecordManager_DNF()
        {
            var target = new RecordData();
            target.PlayerRecords.Add(new cPlayerRecord());
            var actual = LitJson.JsonMapper.ToJson(target);

            Assert.IsFalse(string.IsNullOrWhiteSpace(actual));
        }

        [TestMethod]
        public void RecordManager_RoundTrip_DNF()
        {
            var target = new RecordData();
            target.PlayerRecords.Add(new cPlayerRecord());
            var jsonData = LitJson.JsonMapper.ToJson(target);

            var deserialized = LitJson.JsonMapper.ToObject<RecordData>(jsonData);

            Assert.IsTrue(deserialized != null);
        }

        [TestMethod]
        public void RecordManager_RoundTrip_ProperlyDeserializeFloats()
        {
            var target = new RecordData();
            target.PlayerRecords.Add(new cPlayerRecord() { bestScoreMultiplierFromSecondChance = 3.14f });
            var jsonData = LitJson.JsonMapper.ToJson(target);

            var deserialized = LitJson.JsonMapper.ToObject<RecordData>(jsonData);

            Assert.AreEqual(
                target.PlayerRecords[0].bestScoreMultiplierFromSecondChance,
                deserialized.PlayerRecords[0].bestScoreMultiplierFromSecondChance);
        }

        [TestMethod]
        public void RecordManager_RoundTripWithCultureChange_ProperlyDeserializeFloats()
        {
            var target = new RecordData();
            target.PlayerRecords.Add(new cPlayerRecord() { bestScoreMultiplierFromSecondChance = 3.14f });
            var jsonData = LitJson.JsonMapper.ToJson(target);

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("fr-FR");

            var deserialized = LitJson.JsonMapper.ToObject<RecordData>(jsonData);

            Assert.AreEqual(
                target.PlayerRecords[0].bestScoreMultiplierFromSecondChance,
                deserialized.PlayerRecords[0].bestScoreMultiplierFromSecondChance);
        }

    }
}
