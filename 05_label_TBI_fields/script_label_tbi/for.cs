using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Collections.Generic;
using VMS.TPS.Common.Model.API;
using VMS.TPS.Common.Model.Types;
using System.Windows.Controls;

namespace VMS.TPS
{

    public class Script
    {
        public Script()
        {
        }

        public void Execute(ScriptContext context, Window window)
        {
            var FORUID = context.PlanSetup.Series.FOR;
            var beam06 = context.PlanSetup.Beams
                .FirstOrDefault(beam => beam.Id.Contains("6"));

            var beamNumber = beam06.BeamNumber;
            var isocenter = beam06.IsocenterPosition;

            MessageBox.Show("BeamNumber: " + beamNumber + "\nFORUID: " + FORUID);
        }
    }
}