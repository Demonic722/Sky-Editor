﻿Imports SkyEditorBase

Public Class MyPlugin
    Implements iSkyEditorPlugin
    Public ReadOnly Property PluginAuthor As String Implements iSkyEditorPlugin.PluginAuthor
        Get
            Return "Me!"
        End Get
    End Property

    Public ReadOnly Property PluginName As String Implements iSkyEditorPlugin.PluginName
        Get
            Return "My Plugin!"
        End Get
    End Property
    Public ReadOnly Property Credits As String Implements iSkyEditorPlugin.Credits
        Get
            'In credits, you can use \n for a line break
            Return PluginHelper.GetLanguageItem("MyPluginCredits", "My Plugin\nMade by me.")
        End Get
    End Property

    Public Sub Load(ByRef Manager As PluginManager) Implements iSkyEditorPlugin.Load
        'Register any custom Editor Tabs you've made.  Be sure to pass the TYPE of the tab, not an instance of the tab.
        'The tab should have a default constructor, with no parameters, because a new instance will be made whenever the save is loaded.
        Manager.RegisterEditorTab(GetType(MyTab))

        'Register any save file types
        'If you include this line, you need to include RegisterGameType.
        Manager.RegisterSaveType(GameStrings.MySaveSaveID, GetType(MySave))

        'Register game types for each save type, because some games share the same save type, while having different cheat codes.
        'The key (first argument) should be the specific game, and the value (second argument) should be the save file structure ID.
        'The game ID should include a relevant extension, such as .nds, .gba, or .exe, so that exeternal plugins can run the relevant game if applicable.  If the save isn't for a game, don't include an extension.
        Manager.RegisterGameType(GameStrings.MySaveGameID, GameStrings.MySaveSaveID)

        'In order for someone to be able to open a file, and see the appropriate tabs, without having to select
        'a save type, you should register a detector to detect the save type.
        Manager.RegisterSaveTypeDetector(AddressOf DetectSaveType)

        'Sky Editor has a console that's useful when developing plugins.
        'You can define your own commands here.
        Manager.RegisterConsoleCommand("MyCommand", AddressOf Commands.MyCommand)

        'If you have one, you can put a menu item at the top of the form.
        'It's recommended that you avoid doing so unless you're sure it's needed in some way,
        'to avoid cluttering the menu bar.
        Manager.RegisterMenuItem(New MyMenuItem(Manager))

        'You'll probably want the open file dialog to make your file format visible.
        'With this, you can add your own filter.  First parameter is the filter itself (include the * if applicable),
        'and the second parameter is what to display to the user, so it should be translated.
        Manager.RegisterIOFilter("*.ext", PluginHelper.GetLanguageItem("MySave"))

        'If you want to create cheat codes, you can register your code generator here.
        Manager.RegisterCodeGenerator(New MyCodeGenerator)

        'Some games might not be supported by Sky Editor.
        'In this case, you can create your own code type.
        Manager.RegisterCodeType(New ARDS.CheatFormat("MyFormat", {}))
    End Sub
    Public Sub UnLoad(ByRef Manager As PluginManager) Implements iSkyEditorPlugin.UnLoad
        'Delete any temporary files.  Will be run on form close.
    End Sub

    Public Sub PrepareForDistribution() Implements iSkyEditorPlugin.PrepareForDistribution
        'Delete any files that are not needed for distribution, including ROMs, decompressed files, and user-specific setting files.
        'If no such content exists, there's no need to add anything here.
    End Sub

    Public Function DetectSaveType(File As GenericFile) As String
        'Here, you should determine what kind of save format File is.
        'If it is one of yours, return the save id.
        'If it's not, or you don't know, return nothing to let other detectors do their thing.

        'It does not have to be located in SkyEditorInfo, but your register statement should have access to it.

        'If you know:
        'Return GameStrings.MySaveSaveID
        'If you don't:
        Return Nothing
    End Function

    Public Shared Function GetCheatFormat() As ARDS.CheatFormat
        'Example of defining a new cheat format.
        'No translation required, as it will be translated for you.
        Return New ARDS.CheatFormat("MyFormat", {})
    End Function
End Class
