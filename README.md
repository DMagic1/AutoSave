AutoSave
========

KSP plugin to create rolling series of backup persistent files.


Installation:

Place the AutoSave folder in the main KSP GameData folder.

Change the MaxSaves value in the Settings.cfg file to create more or fewer save backups.

Do not move the Settings.cfg file out of the GameData/AutoSave folder, it will not work otherwise and the plugin will set the default number of three backups.

**Will not work with autoload mods - Plugins that load directly into a save file - Will only create blank "just started" persistent files.

To restore a backup, first rename or move the standard "persistent" (or "persistent.sfs", depending on your explorer view settings) file.

Then remove the "Backup" from the desired file to create a new "persistent" (or "persistent.sfs") file.

