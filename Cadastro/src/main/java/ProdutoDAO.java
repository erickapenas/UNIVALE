import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class ProdutoDAO {
    
    public void insertMySQL(String nome, String categoria, String marca, double peso, double preco, String descricao) {
        MySQLConnection mysql = new MySQLConnection();
        Connection connection = mysql.getConexao();
        if (connection != null) {
            try {
                String campos = "NOME, CATEGORIA, MARCA, PESO, PRECO, DESCRICAO";
                String valores = "?, ?, ?, ?, ?, ?";
                String sql = "INSERT INTO PRODUTOS ("+ campos +") VALUES (" + valores + ")";
                
                PreparedStatement statement = connection.prepareStatement(sql);
                statement.setString(1, nome);
                statement.setString(2, categoria);
                statement.setString(3, marca);
                statement.setDouble(4, peso);
                statement.setDouble(5, preco);
                statement.setString(6, descricao);
                
                int qtdeCad = statement.executeUpdate();
                if (qtdeCad > 0) {
                    System.out.println("Produto cadastrado com sucesso!");
                }
            } catch (SQLException e) {
                e.printStackTrace();
            } finally {
                mysql.desconectar();
            }
        }
    }
    
     public static void selecionarProdutos() {
        MySQLConnection mysql = new MySQLConnection();
        Connection connection = mysql.getConexao();
        if (connection != null) {
            try {
                String query = "SELECT * FROM produto";
                PreparedStatement statement = connection.prepareStatement(query);
                ResultSet resultSet = statement.executeQuery();
                while (resultSet.next()) {
                    System.out.println("Dados: " + resultSet.getString("NOME"));
                }
            } catch (SQLException e) {
                e.printStackTrace();
            } finally {
                mysql.desconectar();
            }
        }
    }
}
