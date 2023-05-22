using stolov.Properties;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stolov {
	internal class QueryManager {
		private static object[,] ResultObjectArray2D;
		private static object[] ResultObjectArray1D;
		private static object ResultObject;
		private static int ResultNonQuery;
		private static string connectionString = Settings.Default.ConnectionString;
		//private string queryString { get; set; }
		private static bool successExecuting = false;

		public static void ExecuteQuery(string queryString) {
			ClearFields();
			SqlConnection connection = new SqlConnection(connectionString);
			SqlCommand command = new SqlCommand(queryString, connection);
			List<object[]> listData = new List<object[]>();
			command.CommandTimeout = 60;
			int VisibleFieldCount = 0;
			connection.Open();
			using (connection) {
				SqlDataReader dataReader = command.ExecuteReader();
				if (dataReader == null) return;
				if (dataReader.HasRows) {
					int RowIndex = 0;
					VisibleFieldCount = dataReader.VisibleFieldCount;
					while (dataReader.Read()) {
						listData.Add(new object[VisibleFieldCount]);
						dataReader.GetValues(listData[RowIndex]);
						RowIndex++;
					}
					successExecuting = true;
				} else return;
			}
			connection.Close();

			if (listData.Count == 1) {
				if (VisibleFieldCount == 1)                 //одна строка и один столбец
				{
					ResultObject = listData[0][0];
					return;
				}

				ResultObjectArray1D = listData[0];          //одна строка и много столбцов
				return;
			}

			if (VisibleFieldCount == 1)                     //много строк и один столбец
			{
				ResultObjectArray1D = new object[listData.Count];
				for (int i = 0; i < listData.Count; i++) ResultObjectArray1D[i] = listData[i][0];
				return;
			}

			ResultObjectArray2D = new object[listData.Count, VisibleFieldCount];     //много строк и много столбцов
			for (int i = 0; i < listData.Count; i++) {
				for (int j = 0; j < VisibleFieldCount; j++) ResultObjectArray2D[i, j] = listData[i][j];
			}
		}

		public static int ExecuteNonQuery(string queryString) {
			ClearFields();
			SqlConnection connection = new SqlConnection(connectionString);
			SqlCommand command = new SqlCommand(queryString, connection);
			command.CommandTimeout = 60;
			connection.Open();
			using (connection) {
				ResultNonQuery = command.ExecuteNonQuery();
			}
			connection.Close();
			if (ResultNonQuery != 0) successExecuting = true;
			return ResultNonQuery;
		}

		private static void ClearFields() {
			ResultObject = null;
			ResultObjectArray1D = null;
			ResultObjectArray2D = null;
			ResultNonQuery = 0;
		}

		public static object GetResultObject() {
			if (ResultObject == null) {
				if (ResultObjectArray1D != null) return ResultObjectArray1D[0];
				if (ResultObjectArray2D != null) return ResultObjectArray2D[0, 0];
			}
			return ResultObject;
		}

		public static object[] GetResultObjectArray1D() {
			if (ResultObjectArray1D == null) {
				if (ResultObjectArray2D != null) {
					ResultObjectArray1D = new object[ResultObjectArray2D.GetLength(1)];
					for (int i = 0; i < ResultObjectArray2D.GetLength(1); i++) ResultObjectArray1D[i] = ResultObjectArray2D[0, i];
					return ResultObjectArray1D;
				}
				if (ResultObject != null) ResultObjectArray1D = new object[1] { ResultObject };

			}
			return ResultObjectArray1D;
		}

		public static object[,] GetResultObjectArray2D() {
			if (ResultObjectArray2D == null) {
				if (ResultObjectArray1D != null) {
					ResultObjectArray2D = new object[1, ResultObjectArray1D.Length];
					for (int i = 0; i < ResultObjectArray1D.Length; i++) ResultObjectArray2D[0, i] = ResultObjectArray1D[i];
					return ResultObjectArray2D;
				}
				if (ResultObject != null) ResultObjectArray2D = new object[1, 1] { { ResultObject } };
			}
			return ResultObjectArray2D;
		}

		public static int GetResultNonQuery() {
			return ResultNonQuery;
		}

		public static bool IsExecuted() {
			return successExecuting;
		}
	}
}
