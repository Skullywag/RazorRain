using System;
using System.Collections.Generic;
using Verse;
namespace RimWorld
{
    public class Thing_RazorSpawner : ThingWithComps
    {
        private int numRazorEventCount;
        private Random random = new Random();
        public override void SpawnSetup()
        {
            this.numRazorEventCount = this.random.Next(20, 30);
            base.SpawnSetup();
        }
        public override void TickRare()
        {
            if (this.random.Next(2) == 1)
            {
                this.DoRazorRainEvent();
            }
        }
        private void DoRazorRainEvent()
        {
            this.numRazorEventCount--;
            if (this.numRazorEventCount < 0)
            {
                Find.LetterStack.ReceiveLetter("Razor rain over", "The Razor rain seems to be letting up.", LetterType.Good);
                this.Destroy(0);
                return;
            }
            int num = this.random.Next(1, 30);
            IntVec3 dropCenter = CellFinderLoose.RandomCellWith((IntVec3 c) => GenGrid.Standable(c) && !Find.RoofGrid.Roofed(c) && !Find.FogGrid.IsFogged(c), 1000);
            ThingDef thingDef = ThingDef.Named("RazorRainIncoming");
            List<Thing> list = new List<Thing>();
            while (list.Count < num)
            {
                Thing item = ThingMaker.MakeThing(thingDef);
                list.Add(item);
            }
            RazorRainUtility.DropThingsNear(dropCenter, list, 1, true, false);
        }
    }
}
