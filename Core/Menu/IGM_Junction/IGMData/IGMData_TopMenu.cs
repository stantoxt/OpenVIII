﻿using Microsoft.Xna.Framework;

namespace OpenVIII
{
    public partial class IGM_Junction
    {
        #region Classes

        private class IGMData_TopMenu : IGMData.Base
        {
            #region Methods

            public static IGMData_TopMenu Create() => Create<IGMData_TopMenu>(4, 1, new IGMDataItem.Box { Pos = new Rectangle(0, 12, 610, 54) }, 4, 1);

            public override bool Inputs_CANCEL()
            {
                if (Memory.PrevState != null &&
                    Damageable.GetCharacterData(out Saves.CharacterData c) && (
                    Memory.PrevState.Characters[c.ID].CurrentHP() > Memory.State.Characters[c.ID].CurrentHP() ||
                    Memory.PrevState.Characters[c.ID].MaxHP() > Memory.State.Characters[c.ID].MaxHP()))
                {
                    IGM_Junction.Data[SectionName.ConfirmChanges].Show();
                    IGM_Junction.SetMode(Mode.ConfirmChanges);
                }
                else
                {
                    base.Inputs_CANCEL();
                    if (Module_main_menu_debug.State == Module_main_menu_debug.MainMenuStates.IGM_Junction)
                    {
                        Module_main_menu_debug.State = Module_main_menu_debug.MainMenuStates.IGM;
                        IGM.Refresh();
                        FadeIn();
                    }
                }

                return true;
            }

            public override bool Inputs_OKAY()
            {
                switch (CURSOR_SELECT)
                {
                    case 0:
                        IGM_Junction.Data[SectionName.TopMenu_Junction].Show();
                        Cursor_Status |= Cursor_Status.Blinking;
                        IGM_Junction.SetMode(Mode.TopMenu_Junction);
                        break;

                    case 1:
                        IGM_Junction.Data[SectionName.TopMenu_Off].Show();
                        Cursor_Status |= Cursor_Status.Blinking;
                        IGM_Junction.SetMode(Mode.TopMenu_Off);
                        break;

                    case 2:
                        IGM_Junction.Data[SectionName.TopMenu_Auto].Show();
                        Cursor_Status |= Cursor_Status.Blinking;
                        IGM_Junction.SetMode(Mode.TopMenu_Auto);
                        break;

                    case 3:
                        IGM_Junction.Data[SectionName.TopMenu_Abilities].Show();
                        Cursor_Status |= Cursor_Status.Blinking;
                        IGM_Junction.SetMode(Mode.Abilities);
                        break;

                    default:
                        return false;
                }
                base.Inputs_OKAY();
                return true;
            }

            public override void Refresh()
            {
                if (Memory.State.Characters != null && Damageable != null && Damageable.GetCharacterData(out Saves.CharacterData c))
                {
                    Font.ColorID color = (c.JunctionnedGFs == Saves.GFflags.None) ? Font.ColorID.Grey : Font.ColorID.White;
                    for (int i = 1; i <= 3; i++)
                    {
                        ((IGMDataItem.Text)ITEM[i, 0]).FontColor = color;
                        BLANKS[i] = c.JunctionnedGFs == Saves.GFflags.None;
                    }
                }
                base.Refresh();
            }

            public override bool Update()
            {
                bool ret = base.Update();
                if (IGM_Junction != null && IGM_Junction.GetMode().Equals(Mode.TopMenu) && Enabled)
                {
                    FF8String Changed = null;
                    switch (CURSOR_SELECT)
                    {
                        case 0:
                            Changed = Strings.Description.Junction;
                            break;

                        case 1:
                            Changed = Strings.Description.Off;
                            break;

                        case 2:
                            Changed = Strings.Description.Auto;
                            break;

                        case 3:
                            Changed = Strings.Description.Ability;
                            break;
                    }
                    if (Changed != null)
                        IGM_Junction.ChangeHelp(Changed);
                }
                return ret;
            }

            protected override void Init()
            {
                base.Init();
                ITEM[0, 0] = new IGMDataItem.Text { Data = Strings.Name.Junction, Pos = SIZE[0] };
                ITEM[1, 0] = new IGMDataItem.Text { Data = Strings.Name.Off, Pos = SIZE[1] };
                ITEM[2, 0] = new IGMDataItem.Text { Data = Strings.Name.Auto, Pos = SIZE[2] };
                ITEM[3, 0] = new IGMDataItem.Text { Data = Strings.Name.Ability, Pos = SIZE[3] };
                Cursor_Status |= Cursor_Status.Enabled;
                Cursor_Status |= Cursor_Status.Horizontal;
                Cursor_Status |= Cursor_Status.Vertical;
            }

            protected override void InitShift(int i, int col, int row)
            {
                base.InitShift(i, col, row);
                SIZE[i].Inflate(-40, -12);
                SIZE[i].Offset(20 + (-20 * (col > 1 ? col : 0)), 0);
            }

            #endregion Methods
        }

        #endregion Classes
    }
}