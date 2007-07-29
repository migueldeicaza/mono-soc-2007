using System;

namespace umbraco.editorControls.dropdownlist
{
	/// <summary>
	/// Summary description for ColorPickerDataType.
	/// </summary>
	public class DropdownListKeysDataType : Cms.BusinessLogic.datatype.BaseDataType, interfaces.IDataType
	{
		private interfaces.IDataEditor _Editor;
		private interfaces.IData _baseData;
		private KeyValuePrevalueEditor _prevalueeditor;

		public override interfaces.IDataEditor DataEditor 
		{
			get
			{
				if (_Editor == null) 
				{
					_Editor = new dropdown(Data,((KeyValuePrevalueEditor)PrevalueEditor).Prevalues);
				}
				return _Editor;
			}
		}

		public override interfaces.IData Data 
		{
			get 
			{
				if (_baseData == null)
					_baseData = new Cms.BusinessLogic.datatype.DefaultData(this);
				return _baseData;
			}
		}
		public override string DataTypeName 
		{
			get {return "Dropdown list, publishing keys";}
		}

		public override Guid Id 
		{
			get {return new Guid("a74ea9e1-8e18-4d2a-8cf6-73c6206c5da6");}
		}

		public override interfaces.IDataPrevalue PrevalueEditor 
		{
			get 
			{
				if (_prevalueeditor == null)
					_prevalueeditor = new KeyValuePrevalueEditor(this);
				return _prevalueeditor;
			}
		}
	}
}
