using System;
using System.Collections.Generic;
using System.Reflection;

namespace FrostAndGunfireItems
{
	// Token: 0x02000024 RID: 36
	public class AdvancedStringDB
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000101 RID: 257 RVA: 0x0000BA40 File Offset: 0x00009C40
		// (set) Token: 0x06000102 RID: 258 RVA: 0x00002463 File Offset: 0x00000663
		public StringTableManager.GungeonSupportedLanguages CurrentLanguage
		{
			get
			{
				return GameManager.Options.CurrentLanguage;
			}
			set
			{
				StringTableManager.SetNewLanguage(value, true);
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000BA5C File Offset: 0x00009C5C
		public AdvancedStringDB()
		{
			StringDB strings = ETGMod.Databases.Strings;
			strings.OnLanguageChanged = (Action<StringTableManager.GungeonSupportedLanguages>)Delegate.Combine(strings.OnLanguageChanged, new Action<StringTableManager.GungeonSupportedLanguages>(this.LanguageChanged));
			this.Core = new AdvancedStringDBTable(() => StringTableManager.CoreTable);
			this.Items = new AdvancedStringDBTable(() => StringTableManager.ItemTable);
			this.Enemies = new AdvancedStringDBTable(() => StringTableManager.EnemyTable);
			this.Intro = new AdvancedStringDBTable(() => StringTableManager.IntroTable);
			this.Synergies = new AdvancedStringDBTable(() => AdvancedStringDB.SynergyTable);
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000BB6C File Offset: 0x00009D6C
		public void LanguageChanged(StringTableManager.GungeonSupportedLanguages newLang)
		{
			this.Core.LanguageChanged();
			this.Items.LanguageChanged();
			this.Enemies.LanguageChanged();
			this.Intro.LanguageChanged();
			this.Synergies.LanguageChanged();
			Action<StringTableManager.GungeonSupportedLanguages> onLanguageChanged = this.OnLanguageChanged;
			bool flag = onLanguageChanged == null;
			if (!flag)
			{
				onLanguageChanged(newLang);
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000105 RID: 261 RVA: 0x0000BBD0 File Offset: 0x00009DD0
		public static Dictionary<string, StringTableManager.StringCollection> SynergyTable
		{
			get
			{
				StringTableManager.GetSynergyString("ThisExistsOnlyToLoadTables", -1);
				return (Dictionary<string, StringTableManager.StringCollection>)AdvancedStringDB.m_synergyTable.GetValue(null);
			}
		}

		// Token: 0x040000BB RID: 187
		public readonly AdvancedStringDBTable Core;

		// Token: 0x040000BC RID: 188
		public readonly AdvancedStringDBTable Items;

		// Token: 0x040000BD RID: 189
		public readonly AdvancedStringDBTable Enemies;

		// Token: 0x040000BE RID: 190
		public readonly AdvancedStringDBTable Intro;

		// Token: 0x040000BF RID: 191
		public readonly AdvancedStringDBTable Synergies;

		// Token: 0x040000C0 RID: 192
		public static FieldInfo m_synergyTable = typeof(StringTableManager).GetField("m_synergyTable", BindingFlags.Static | BindingFlags.NonPublic);

		// Token: 0x040000C1 RID: 193
		public Action<StringTableManager.GungeonSupportedLanguages> OnLanguageChanged;
	}
}
