'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Option Strict On
Option Explicit On

Namespace DotNetNuke.Modules.Forum

	''' <summary>
	''' A Helper class used to build the post/thread search self contained within the module. This is also the basis for "My Posts". 
	''' </summary>
	''' <remarks></remarks>
	Public Class SearchTerms

#Region "Structure"

		''' <summary>
		''' SearchTerms represent logic used to build the comparative parts of an SQL query. 
		''' </summary>
		''' <remarks></remarks>
		Public Structure SearchTerm

#Region "Members"

			Friend mField As String
			Friend mValue As String
			Friend mCondition As String
			Friend mCompareOperator As CompareOperator

#End Region

#Region "Methods"

			''' <summary>
			''' Sets the property values.
			''' </summary>
			''' <param name="Field"></param>
			''' <param name="Operator"></param>
			''' <param name="Value"></param>
			''' <param name="Condition"></param>
			''' <remarks></remarks>
			Friend Sub SetProperty(ByVal Field As String, ByVal [Operator] As CompareOperator, ByVal Value As String, ByVal Condition As String)
				mField = Field
				mValue = Value
				mCompareOperator = [Operator]
				mCondition = Condition
			End Sub

#End Region

#Region "Properties"

			''' <summary>
			''' The field we are working with (typically, this is a column). 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public Property Field() As String
				Get
					Return mField
				End Get
				Set(ByVal value As String)
					mField = value
				End Set
			End Property

			''' <summary>
			''' The value we are trying to match in our SQL.
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property Value() As String
				Get
					Return mValue
				End Get
			End Property

			''' <summary>
			''' The compare operator used in our SQL (this will be =, >, etc.). 
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property CompareOperator() As CompareOperator
				Get
					Return mCompareOperator
				End Get
			End Property

			''' <summary>
			''' The SQL condition.
			''' </summary>
			''' <value></value>
			''' <returns></returns>
			''' <remarks></remarks>
			Public ReadOnly Property Condition() As String
				Get
					Return mCondition
				End Get
			End Property

#End Region

		End Structure

#End Region

#Region "Constructor"

		''' <summary>
		''' Constructor
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region

#Region "Private Members"

		Private _SearchTerms As New ArrayList
		Private _WhereClause As String

#End Region

#Region "Public Methods"

		''' <summary>
		''' Used by other classes to build the search terms. 
		''' </summary>
		''' <param name="Field">The column being searched against.</param>
		''' <param name="Operator">The compare type operator being used for the search.</param>
		''' <param name="Value">The value the column should be searched for.</param>
		''' <param name="Condition">If the condition should be "AND", "OR", or nothing.</param>
		''' <remarks></remarks>
		Public Sub AddSearchTerm( _
		    ByVal Field As String, _
		    ByVal [Operator] As CompareOperator, _
		    ByVal Value As String, _
		    Optional ByVal Condition As String = " AND ")

			If Value <> String.Empty Then
				Dim sec As New DotNetNuke.Security.PortalSecurity
				Value = sec.InputFilter(Value, PortalSecurity.FilterFlag.NoMarkup Xor PortalSecurity.FilterFlag.NoScripting Xor PortalSecurity.FilterFlag.NoSQL)
			End If

			Dim term As New SearchTerm
			term.SetProperty(Field, [Operator], Value, Condition)

			_SearchTerms.Add(term)
		End Sub

#End Region

#Region "Public Properties"

		''' <summary>
		''' A collection of items to use for searching.
		''' </summary>
		''' <value></value>
		''' <returns>An arraylist of items to search with.</returns>
		''' <remarks></remarks>
		Public Property SearchTerms() As ArrayList
			Get
				Return _SearchTerms
			End Get
			Set(ByVal Value As ArrayList)
				_SearchTerms = Value
			End Set
		End Property

		''' <summary>
		''' The 'WHERE' clause used for the search.
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property WhereClause() As String
			Get
				Dim Term As SearchTerm
				Dim sb As New System.Text.StringBuilder

				For Each Term In SearchTerms
					sb.Append(ConvertOperator(Term))
				Next

				Return sb.ToString
			End Get
		End Property

#End Region

#Region "Private Methods"

		''' <summary>
		''' Uses the compare operator along w/ the search term to generate part of a where clause.
		''' </summary>
		''' <param name="Term"></param>
		''' <returns>A properly formated section of a 'WHERE' clause.</returns>
		''' <remarks></remarks>
		Private Function ConvertOperator(ByVal Term As SearchTerm) As String
			Dim sb As New System.Text.StringBuilder
			Dim hasEnd As Boolean = False
			If Term.Field.EndsWith(")") Then
				Term.Field = Term.Field.TrimEnd(")"c)
				hasEnd = True
			End If
			Select Case Term.CompareOperator
				Case CompareOperator.Equal
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" = ")
					sb.Append(Term.Value)
					sb.Append(")")
				Case CompareOperator.EqualString
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" = N'")
					sb.Append(Term.Value)
					sb.Append("')")
				Case CompareOperator.NotEqual
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" <> ")
					sb.Append(Term.Value)
					sb.Append(")")
				Case CompareOperator.NotEqualString
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" <> N'")
					sb.Append(Term.Value)
					sb.Append("')")
				Case CompareOperator.Contains
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" LIKE N':perc:")

					'' we need to split words here
					'Dim arr As Array = Term.Value.Split(CChar(" "))
					'For Each s As String In arr
					sb.Append(FormatQuery(Term.Value))
					sb.Append(":perc:'")
					'Next

					sb.Append(")")
				Case CompareOperator.StartWith
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" LIKE N'")
					sb.Append(FormatQuery(Term.Value))
					sb.Append(":perc:'")
					sb.Append(")")
				Case CompareOperator.NotStartWith
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" NOT LIKE N'")
					sb.Append(FormatQuery(Term.Value))
					sb.Append(":perc:'")
					sb.Append(")")
				Case CompareOperator.GreaterThan
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" > '")
					sb.Append(Term.Value)
					sb.Append("'")
					sb.Append(")")
				Case CompareOperator.LessThan
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" < '")
					sb.Append(Term.Value)
					sb.Append("'")
					sb.Append(")")
				Case CompareOperator.Between
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" Between ")
					sb.Append(Term.Value)
					sb.Append(")")
				Case CompareOperator.HaveValueIn
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" IN ")
					sb.Append("(" & Term.Value & ")")
					sb.Append(")")
				Case CompareOperator.NotHaveValueIn
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" NOT IN ")
					sb.Append("(" & Term.Value & ")")
					sb.Append(")")
				Case CompareOperator.GreaterThanDate
					sb.Append(Term.Condition & "(")
					sb.Append(Term.Field)
					sb.Append(" > ")
					sb.Append(Term.Value)
					sb.Append(")")
				Case CompareOperator.LeftParentes
					sb.Append("(")
				Case CompareOperator.RightParentes
					sb.Append(")")
				Case CompareOperator.And
					sb.Append(" AND ")
			End Select

			If hasEnd Then
				sb.Append(")")
			End If
			Return sb.ToString
		End Function

		''' <summary>
		''' Used to make sure query generated in code will function properly in SQL.
		''' </summary>
		''' <param name="Query"></param>
		''' <returns>A string of SQL that will run in MS SQL Server.</returns>
		''' <remarks></remarks>
		Private Function FormatQuery(ByVal Query As String) As String
			Dim strReturn As String = Query
			strReturn = Query.Replace("'", "''")
			strReturn = strReturn.Replace("%", "[%]")
			strReturn = strReturn.Replace("-", "[-]")
			Return strReturn
		End Function

#End Region

	End Class

End Namespace