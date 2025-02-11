﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Windows.Win32.System.Com;
using Windows.Win32.System.Variant;
using Windows.Win32.UI.Accessibility;

internal static partial class Interop
{
    internal static partial class UiaCore
    {
        public unsafe class StubFragmentRoot : IRawElementProviderFragmentRoot
        {
            private StubFragmentRoot() { }

            public static StubFragmentRoot Instance { get; } = new();

            object? IRawElementProviderFragmentRoot.ElementProviderFromPoint(double x, double y) => default;
            object? IRawElementProviderFragmentRoot.GetFocus() => default;

            HRESULT IRawElementProviderFragment.Interface.Navigate(NavigateDirection direction, IRawElementProviderFragment** pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderFragment.Interface.GetRuntimeId(SAFEARRAY** pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderFragment.Interface.get_BoundingRectangle(UiaRect* pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderFragment.Interface.GetEmbeddedFragmentRoots(SAFEARRAY** pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderFragment.Interface.SetFocus() => HRESULT.S_OK;

            HRESULT IRawElementProviderFragment.Interface.get_FragmentRoot(Windows.Win32.UI.Accessibility.IRawElementProviderFragmentRoot** pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderSimple.Interface.get_ProviderOptions(ProviderOptions* pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderSimple.Interface.GetPatternProvider(UIA_PATTERN_ID patternId, IUnknown** pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderSimple.Interface.GetPropertyValue(UIA_PROPERTY_ID propertyId, VARIANT* pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }

            HRESULT IRawElementProviderSimple.Interface.get_HostRawElementProvider(IRawElementProviderSimple** pRetVal)
            {
                if (pRetVal is null)
                {
                    return HRESULT.E_POINTER;
                }

                *pRetVal = default;
                return HRESULT.S_OK;
            }
        }
    }
}
