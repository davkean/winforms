﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable disable

using static Interop;

namespace System.Windows.Forms.PropertyGridInternal
{
    internal partial class PropertyDescriptorGridEntry
    {
        protected class PropertyDescriptorGridEntryAccessibleObject : GridEntryAccessibleObject
        {
            private readonly PropertyDescriptorGridEntry _owningPropertyDescriptorGridEntry;

            public PropertyDescriptorGridEntryAccessibleObject(PropertyDescriptorGridEntry owner) : base(owner)
            {
                _owningPropertyDescriptorGridEntry = owner;
            }

            internal override bool IsIAccessibleExSupported() => true;

            /// <summary>
            ///  Returns the element in the specified direction.
            /// </summary>
            /// <param name="direction">Indicates the direction in which to navigate.</param>
            /// <returns>Returns the element in the specified direction.</returns>
            internal override UiaCore.IRawElementProviderFragment FragmentNavigate(UiaCore.NavigateDirection direction)
            {
                switch (direction)
                {
                    case UiaCore.NavigateDirection.NextSibling:
                        var propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)Parent;
                        var propertyGridView = propertyGridViewAccessibleObject.Owner as PropertyGridView;
                        bool currentGridEntryFound = false;
                        return propertyGridViewAccessibleObject.GetNextGridEntry(_owningPropertyDescriptorGridEntry, propertyGridView.TopLevelGridEntries, out currentGridEntryFound);
                    case UiaCore.NavigateDirection.PreviousSibling:
                        propertyGridViewAccessibleObject = (PropertyGridView.PropertyGridViewAccessibleObject)Parent;
                        propertyGridView = propertyGridViewAccessibleObject.Owner as PropertyGridView;
                        currentGridEntryFound = false;
                        return propertyGridViewAccessibleObject.GetPreviousGridEntry(_owningPropertyDescriptorGridEntry, propertyGridView.TopLevelGridEntries, out currentGridEntryFound);
                    case UiaCore.NavigateDirection.FirstChild:
                        return GetFirstChild();
                    case UiaCore.NavigateDirection.LastChild:
                        return GetLastChild();
                }

                return base.FragmentNavigate(direction);
            }

            private UiaCore.IRawElementProviderFragment GetFirstChild()
            {
                if (_owningPropertyDescriptorGridEntry is null)
                {
                    return null;
                }

                if (_owningPropertyDescriptorGridEntry.ChildCount > 0)
                {
                    return _owningPropertyDescriptorGridEntry.Children.GetEntry(0).AccessibilityObject;
                }

                var propertyGridView = GetPropertyGridView();
                if (propertyGridView is null)
                {
                    return null;
                }

                var selectedGridEntry = propertyGridView.SelectedGridEntry;
                if (_owningPropertyDescriptorGridEntry == selectedGridEntry)
                {
                    if (selectedGridEntry.Enumerable &&
                        propertyGridView.DropDownVisible &&
                        propertyGridView.DropDownControlHolder?.Component == propertyGridView.DropDownListBox)
                    {
                        return propertyGridView.DropDownListBoxAccessibleObject;
                    }

                    if (propertyGridView.DropDownVisible && propertyGridView.DropDownControlHolder is not null)
                    {
                        return propertyGridView.DropDownControlHolder.AccessibilityObject;
                    }

                    return propertyGridView.EditAccessibleObject;
                }

                return null;
            }

            private UiaCore.IRawElementProviderFragment GetLastChild()
            {
                if (_owningPropertyDescriptorGridEntry is null)
                {
                    return null;
                }

                if (_owningPropertyDescriptorGridEntry.ChildCount > 0)
                {
                    return _owningPropertyDescriptorGridEntry.Children
                        .GetEntry(_owningPropertyDescriptorGridEntry.ChildCount - 1).AccessibilityObject;
                }

                var propertyGridView = GetPropertyGridView();
                if (propertyGridView is null)
                {
                    return null;
                }

                var selectedGridEntry = propertyGridView.SelectedGridEntry;
                if (_owningPropertyDescriptorGridEntry == selectedGridEntry)
                {
                    if (selectedGridEntry.Enumerable && propertyGridView.DropDownButton.Visible)
                    {
                        return propertyGridView.DropDownButton.AccessibilityObject;
                    }

                    return propertyGridView.EditAccessibleObject;
                }

                return null;
            }

            private PropertyGridView GetPropertyGridView()
            {
                var propertyGridViewAccessibleObject = Parent as PropertyGridView.PropertyGridViewAccessibleObject;
                if (propertyGridViewAccessibleObject is null)
                {
                    return null;
                }

                return propertyGridViewAccessibleObject.Owner as PropertyGridView;
            }

            internal override bool IsPatternSupported(UiaCore.UIA patternId)
            {
                if (patternId == UiaCore.UIA.ValuePatternId ||
                    (patternId == UiaCore.UIA.ExpandCollapsePatternId && owner.Enumerable))
                {
                    return true;
                }

                return base.IsPatternSupported(patternId);
            }

            internal override void Expand()
            {
                if (ExpandCollapseState == UiaCore.ExpandCollapseState.Collapsed)
                {
                    ExpandOrCollapse();
                }
            }

            internal override void Collapse()
            {
                if (ExpandCollapseState == UiaCore.ExpandCollapseState.Expanded)
                {
                    ExpandOrCollapse();
                }
            }

            private void ExpandOrCollapse()
            {
                if (!GetPropertyGridView().IsHandleCreated)
                {
                    return;
                }

                var propertyGridView = GetPropertyGridView();
                if (propertyGridView is null)
                {
                    return;
                }

                int row = propertyGridView.GetRowFromGridEntry(_owningPropertyDescriptorGridEntry);

                if (row != -1)
                {
                    propertyGridView.PopupDialog(row);
                }
            }

            internal override UiaCore.ExpandCollapseState ExpandCollapseState
            {
                get
                {
                    var propertyGridView = GetPropertyGridView();
                    if (propertyGridView is null)
                    {
                        return UiaCore.ExpandCollapseState.Collapsed;
                    }

                    if (_owningPropertyDescriptorGridEntry == propertyGridView.SelectedGridEntry &&
                        ((_owningPropertyDescriptorGridEntry is not null && _owningPropertyDescriptorGridEntry.InternalExpanded)
                         || propertyGridView.DropDownVisible))
                    {
                        return UiaCore.ExpandCollapseState.Expanded;
                    }

                    return UiaCore.ExpandCollapseState.Collapsed;
                }
            }

            internal override object GetPropertyValue(UiaCore.UIA propertyID)
            {
                if (propertyID == UiaCore.UIA.IsEnabledPropertyId)
                {
                    return !((PropertyDescriptorGridEntry)owner).IsPropertyReadOnly;
                }
                else if (propertyID == UiaCore.UIA.LegacyIAccessibleDefaultActionPropertyId)
                {
                    return string.Empty;
                }
                else if (propertyID == UiaCore.UIA.IsValuePatternAvailablePropertyId)
                {
                    return true;
                }

                return base.GetPropertyValue(propertyID);
            }
        }
    }
}
