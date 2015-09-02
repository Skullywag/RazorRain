using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace RimWorld
{
    public static class RazorRainUtility
    {
        private static List<List<Thing>> tempList = new List<List<Thing>>();
        public static void MakeRazorRainAt(IntVec3 loc)
        {
            RazorRainIncoming razorIncoming = (RazorRainIncoming)ThingMaker.MakeThing(ThingDef.Named("RazorRainIncoming"));
            GenSpawn.Spawn(razorIncoming, loc);
        }
        public static void DropThingsNear(IntVec3 dropCenter, IEnumerable<Thing> things, int openDelay = 30, bool canInstaDropDuringInit = true, bool leaveSlag = false)
        {
            foreach (Thing current in things)
            {
                List<Thing> list = new List<Thing>();
                list.Add(current);
                RazorRainUtility.tempList.Add(list);
            }
            RazorRainUtility.DropThingGroupsNear(dropCenter, RazorRainUtility.tempList, openDelay, canInstaDropDuringInit, leaveSlag);
            RazorRainUtility.tempList.Clear();
        }
        public static void DropThingGroupsNear(IntVec3 dropCenter, List<List<Thing>> thingsGroups, int openDelay = 30, bool canInstaDropDuringInit = true, bool leaveSlag = false, bool canRoofPunch = true)
        {
            foreach (List<Thing> current in thingsGroups)
            {
                IntVec3 intVec;
                if (!DropCellFinder.TryFindDropSpotNear(dropCenter, out intVec, true, canRoofPunch))
                {
                    Log.Warning(string.Concat(new object[]
					{
						"DropThingsNear failed to find a place to drop ",
						current.FirstOrDefault<Thing>(),
						" near ",
						dropCenter,
						". Dropping on random square instead."
					}));
                    intVec = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
                }
                foreach (Thing current2 in current)
                {
                    ThingWithComps thingWithComponents = current2 as ThingWithComps;
                    if (thingWithComponents != null && thingWithComponents.GetComp<CompForbiddable>() != null)
                    {
                        thingWithComponents.GetComp<CompForbiddable>().Forbidden = true;
                    }
                }
                if (canInstaDropDuringInit && Find.TickManager.TicksGame < 2)
                {
                    using (List<Thing>.Enumerator enumerator3 = current.GetEnumerator())
                    {
                        while (enumerator3.MoveNext())
                        {
                            Thing current3 = enumerator3.Current;
                            GenPlace.TryPlaceThing(current3, intVec, ThingPlaceMode.Near);
                        }
                        continue;
                    }
                }
                intVec = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
                RazorRainUtility.MakeRazorRainAt(intVec);
            }
        }
    }
}
