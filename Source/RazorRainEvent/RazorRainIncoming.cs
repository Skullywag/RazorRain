using System;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace RimWorld
{
    public class RazorRainIncoming : Thing
    {
        protected int ticksToImpact = 150;
        private bool soundPlayed;
        private System.Random random = new System.Random();
        private static readonly SoundDef LandSound = SoundDef.Named("MortarRound_PreImpact");
        public override Vector3 DrawPos
        {
            get
            {
                Vector3 result = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FlyingItem);
                float num = (float)(this.ticksToImpact * this.ticksToImpact) * 0.01f;
                result.x -= num * 0.8f;
                result.z += num * 0.6f;
                return result;
            }
        }
        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.ticksToImpact = UnityEngine.Random.Range(120, 200);
        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.ticksToImpact, "ticksToImpact", 0, false);
        }
        public override void Tick()
        {
            MoteThrower.ThrowSmoke(DrawPos, 0.5f);
            MoteThrower.ThrowLightningGlow(DrawPos, 0.5f);

            this.ticksToImpact--;
            if (!this.soundPlayed && this.ticksToImpact < 100)
            {
                this.soundPlayed = true;
                SoundStarter.PlayOneShot(RazorRainIncoming.LandSound, base.Position);
            }
            if (this.ticksToImpact <= 0)
            {
                GenExplosion.DoExplosion(base.Position, 1f, DamageDefOf.Bomb, null, null, null);
                Thing razor = ThingMaker.MakeThing(ThingDef.Named("Steel"));
                razor.SetForbidden(true);
                int num = this.random.Next(1, 8);
                if (num == 5)
                {
                    GenSpawn.Spawn(razor, base.Position, this.Rotation);
                }               
                this.Destroy(DestroyMode.Vanish);
            }    
        }

        public override void DrawAt(Vector3 drawLoc)
        {
            base.DrawAt(drawLoc);
            float num = 2f + (float)this.ticksToImpact / 100f;
            Vector3 s = new Vector3(num, 1f, num);
            Matrix4x4 matrix = default(Matrix4x4);
            drawLoc.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
            matrix.SetTRS(this.TrueCenter(), base.Rotation.AsQuat, s);
        }
    }
}
