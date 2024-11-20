using iSukces.Build;

namespace Build.XEducation;

public static class IgnoreWarnings
{
    public static IEnumerable<string> Get()
    {

        yield return KnownCompilerDirectives.Cs0612_ClassDesignerMarkedMemberWithObsoleteAttribute;
        yield return KnownCompilerDirectives.Cs0618_ClassMemberWasMarkedWithObsoleteAttribute;
        yield return KnownCompilerDirectives.Cs1591_MissingXmlCommentForPubliclyVisibleTypeOrMember;
        yield return KnownCompilerDirectives.Cs8625_CannotConvertNullLiteralToNonNullableReferenceType;

        // The type or namespace name 'Xaml' does not exist in the namespace 'iSukces.Ui.Cg'
        yield return KnownCompilerDirectives.Cs0234_TypeOrNamespaceNameXamlDoesNotExistInNamespace;
        // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        yield return KnownCompilerDirectives
            .Cs8632_AnnotationForNullableReferenceTypesShouldOnlyBeUsedInCodeWithinNullableAnnotationsContext;
        // Cannot convert null literal to non-nullable reference type
        yield return KnownCompilerDirectives.Cs8625_CannotConvertNullLiteralToNonNullableReferenceType;

        // OLD NET FRAMEWORK
        yield return KnownCompilerDirectives.NU1701_PackageWasRestoredUsingOldFrameworkPackageMayNotBeFullyCompatibleWithYourProject;

        // CS8600: Converting null literal or possible null value to non-nullable type. [C:\programs\ALPEX\XEducation\app\Pd.Protocols.Data\Pd.Protocols.Data.csproj]
        yield return KnownCompilerDirectives.Cs8600_ConvertingNullLiteralOrPossibleNullValueToNonNullableType;

        // CS8601: Possible null reference assignment
        yield return KnownCompilerDirectives.Cs8601_PossibleNullReferenceAssignment;

        // CS8602: Dereference of a possibly null reference. [C:\programs\ALPEX\XEducation\app\Pd.Protocols.Data\Pd.Protocols.Data.csproj]
        yield return KnownCompilerDirectives.Cs8602_DereferenceOfPossiblyNullReference;
        // CS8603: Possible null reference return. [C:\programs\ALPEX\XEducation\app\Pd.Protocols.Data\Pd.Protocols.Data.csproj]
        yield return KnownCompilerDirectives.Cs8603_PossibleNullReferenceReturn;
        // CS8604: Possible null reference argument for parameter 
        yield return KnownCompilerDirectives.Cs8604_PossibleNullReferenceArgumentForParameter;
        
        // CS8605: Unboxing a possibly null value.
        yield return KnownCompilerDirectives.Cs8605_UnboxingPossiblyNullValue;
        
        
        // CS8612: Nullability of reference types in type of 
        yield return KnownCompilerDirectives.Cs8612_NullabilityOfReferenceTypesInTypeOf;
        
        // CS8618: Non-nullable property 'Objects' must contain a non-null value when exiting constructor.
        yield return KnownCompilerDirectives.Cs8618_NonNullablePropertyObjectsMustContainNonNullValueWhenExitingConstructor;
        
        //warning CS8619: Nullability of reference types in value of type 
        yield return KnownCompilerDirectives.Cs8619_NullabilityOfReferenceTypesInValueOfType;
        
        
        // CS8621: Nullability of reference types in return type of 'lambda expression'
        yield return KnownCompilerDirectives.Cs8621_NullabilityOfReferenceTypesInReturnTypeOfLambdaExpression;
        
        // CS8622: Nullability of reference types in type of parameter
        yield return KnownCompilerDirectives.Cs8622_NullabilityOfReferenceTypesInTypeOfParameter;


        yield return KnownCompilerDirectives
            .Cs8765_NullabilityOfTypeOfParameterDoesnTMatchOverriddenMemberPossiblyBecauseOfNullabilityAttributes;
        yield return KnownCompilerDirectives
            .Cs8766_NullabilityOfReferenceTypesInReturnTypeOfDoesnTMatchImplicitlyImplementedMemberPossiblyBecauseOfNullabilityAttributes;
        // CS8767: Nullability of reference types in type of parameter
        yield return KnownCompilerDirectives
            .Cs8767_NullabilityOfReferenceTypesInTypeOfParameterOfDoesnTMatchImplicitlyImplementedMemberPossiblyBecauseOfNullabilityAttributes;
        
        // CA1416: This call site is reachable on all platforms
        yield return KnownCompilerDirectives.CA1416_CallSiteIsReachableOnAllPlatforms;
        
        // CS8714: The type 'TKey' cannot be used as type parameter 'TKey' in the generic type or method 'Dictionary<TKey, TValue
        yield return KnownCompilerDirectives.Cs8714_TypeTKeyCannotBeUsedAsTypeParameterTKeyInGenericTypeOrMethodDictionaryTKeyTValue;
        
        
        // CS1998: This async method lacks 'await'
        yield return KnownCompilerDirectives.Cs1998_AsyncMethodLacksAwait;
        
        
        
        // CS0067: The event 'TrackWiresTools.GetInfoFromVisualElement' is never used
        yield return KnownCompilerDirectives.Cs0067_EventTrackWiresToolsGetInfoFromVisualElementIsNeverUsed;
        
        // CS0809: Obsolete member 'BentPipe.PipeCircumferenceSegments' overrides non-obsolete member 'BentOrStraightPipe.PipeCircumferenceSegments'
        yield return KnownCompilerDirectives
            .Cs0809_ObsoleteMemberBentPipePipeCircumferenceSegmentsOverridesNonObsoleteMemberBentOrStraightPipePipeCircumferenceSegments;
        
        
        // CS0184: The given expression is never of the provided ('xxx') type        
        yield return KnownCompilerDirectives.Cs0184_GivenExpressionIsNeverOfProvidedXxxType;
        
        
        // CS8634: The type 'System.IO.DirectoryInfo?' cannot be used as type parameter 'T' in the generic type or method 'iSukces_Code_Translations_Extensions.
        yield return KnownCompilerDirectives.Cs8634_TypeSystemIODirectoryInfoCannotBeUsedAsTypeParameterTInGenericTypeOrMethod;
        
        // CS3021: 'WinNativeMethods.GetWindowStyle(nint)' does not need a CLSCompliant attribute because the assembly does not have a CLSCompliant attribute [C:\programs\ALPEX\XEducation\app\!iSukces.Ui\iSukces.Ui.Winforms\iSukces.Ui.Winforms.csproj]
        yield return KnownCompilerDirectives
            .Cs3021_WinNativeMethodsGetWindowStyleNintDoesNotNeedCLSCompliantAttributeBecauseAssemblyDoesNotHaveCLSCompliantAttribute;
        
        
        // CS8597: Thrown value may be null. [C:\programs\ALPEX\XEducation\app\!iSukces.Ui\iSukces.Ui.Winforms\iSukces.Ui.Winforms.csproj]
        yield return KnownCompilerDirectives.Cs8597_ThrownValueMayBeNull;
        
        
        // CS8629: Nullable value type may be null. [C:\programs\ALPEX\XEducation\app\Pd.Cad\Pd.Cad.csproj]
        yield return KnownCompilerDirectives.Cs8629_NullableValueTypeMayBeNull;
        
        
        // CS1522: Empty switch block
        yield return KnownCompilerDirectives.Cs1522_EmptySwitchBlock;
        
        // CS1066: The default value specified for parameter 'startingKind' will have no effect because it applies to a member that is used in contexts that do not allow optional arguments [C:\programs\ALPEX\XEducation\app\Pd.Cad\Pd.Cad.csproj]
        yield return KnownCompilerDirectives
            .Cs1066_DefaultValueSpecifiedForParameterStartingKindWillHaveNoEffectBecauseItAppliesToMemberThatIsUsedInContextsThatDoNotAllowOptionalArguments;
        
        
        // CS8620: Argument of type 'Func<object, Task>' cannot be used for parameter 'execute' of type 'Func<object?, Task>' in 'AsyncCommand.AsyncCommand(Func<object?,
        yield return KnownCompilerDirectives.Cs8620_ArgumentCannotBeUsedForParameterDueToDifferencesInNullabilityOfReferenceTypes;
        
        
        // CS4014: Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call. [C:\programs\ALPEX\XEducation\app\Pd.Injector.Common\Pd.Injector.Common.csproj]
        yield return KnownCompilerDirectives
            .Cs4014_BecauseCallIsNotAwaitedExecutionOfCurrentMethodContinuesBeforeCallIsCompletedConsiderApplyingAwaitOperatorToResultOfCall;
        
        
        // CS0282: There is no defined ordering between fields in multiple declarations of partial struct 
        yield return KnownCompilerDirectives.Cs0282_ThereIsNoDefinedOrderingBetweenFieldsInMultipleDeclarationsOfPartialStruct;


        //CS8981: The type name 'alglib' only contains lower-cased ascii characters. Such names may become reserved for the language. [C:\programs\ALPEX\XEducation\app\Pd.Mes\Pd.Mes.csproj]
        yield return "8981"; // z tym raczej nic nie zrobię
        
        // CS0672: Member 'InvalidOrIncompleteModelException.GetObjectData(SerializationInfo, StreamingContext)' overrides obsolete member 'Exception.GetObjectData(SerializationInfo, StreamingContext)'. Add the Obsolete attribute to 'InvalidOrIncompleteModelException.GetOb
        yield return "0672"; // To zdecydowanie do poprawy

        yield return "1030"; // !!!!!!!!!!!!!!! Własne
    }
}
