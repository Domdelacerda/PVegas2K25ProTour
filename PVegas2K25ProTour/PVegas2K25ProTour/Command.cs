using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PVegas2K25ProTour
{
    abstract class Command
    {
        protected GameControl game_control;
        public Command(GameControl gc) { game_control = gc; }
        public abstract void execute();
        public abstract void unexecute();
    }

    class IncreaseCommand : Command
    {
        private int settings_type;
        public IncreaseCommand(GameControl gc, int type) : base(gc)
        {
            settings_type = type;
        }
        public override void execute()
        {
            // To do: receiver.action()
            game_control.AdjustSettingVal(settings_type);
        }
        public override void unexecute()
        {
            // To do: receiver.action() 
            game_control.AdjustSettingVal(game_control.choiceSwitch(settings_type));
        }
        
    }
    class DecreaseCommand : Command
    {
        private int settings_type;
        public DecreaseCommand(GameControl gc, int type) : base(gc)
        {
            settings_type = type;
        }
        public override void execute()
        {
            // To do: receiver.action()
            game_control.AdjustSettingVal(settings_type);
        }
        public override void unexecute()
        {
            // To do: receiver.action() 
            game_control.AdjustSettingVal(game_control.choiceSwitch(settings_type));
        }

    }
}
