﻿using Anathema.MemoryManagement;
using Anathema.MemoryManagement.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Anathema
{
    delegate void TableAddressEntryEditorEventHandler(Object Sender, TableAddressEntryEditorEventArgs Args);
    class TableAddressEntryEditorEventArgs : EventArgs
    {

    }

    interface ITableAddressEntryEditorView : IView
    {
        // Methods invoked by the presenter (upstream)
    }

    interface ITableAddressEntryEditorModel : IModel
    {
        // Events triggered by the model (upstream)

        // Functions invoked by presenter (downstream)
        void AddTableEntryItem(Int32 MainSelection, Int32[] SelectedIndicies, AddressItem AddressItem);
    }

    class TableAddressEntryEditorPresenter : Presenter<ITableAddressEntryEditorView, ITableAddressEntryEditorModel>
    {
        protected new ITableAddressEntryEditorView View { get; set; }
        protected new ITableAddressEntryEditorModel Model { get; set; }

        public TableAddressEntryEditorPresenter(ITableAddressEntryEditorView View, ITableAddressEntryEditorModel Model) : base(View, Model)
        {
            this.View = View;
            this.Model = Model;
        }

        #region Method definitions called by the view (downstream)

        public void AcceptChanges(Int32 MainSelection, Int32[] SelectedIndicies, String Description, String Address, String ValueType, String Value, String[] Offsets, Boolean IsHex)
        {
            // Convert passed parameters to the appropriate types to construct an AddressItem
            List<Int32> OffsetsInt = new List<int>();
            foreach (String Offset in Offsets)
                OffsetsInt.Add((Int32)Conversions.AddressToValue(Offset));

            AddressItem AddressItem = new AddressItem(Conversions.AddressToValue(Address).ToIntPtr(), Conversions.StringToPrimitiveType(ValueType), Description,
                 OffsetsInt.ToArray(), IsHex);

            if (CheckSyntax.CanParseValue(Conversions.StringToPrimitiveType(ValueType), Value))
                AddressItem.Value = Conversions.ParseValue(Conversions.StringToPrimitiveType(ValueType), Value);
            else
                AddressItem.Value = null;

            // Pass constructed item to model to send to the table
            Model.AddTableEntryItem(MainSelection, SelectedIndicies, AddressItem);
        }

        #endregion

        #region Event definitions for events triggered by the model (upstream)

        #endregion
    }
}