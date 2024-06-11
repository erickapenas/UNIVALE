public class Produto extends ProdutoDAO{
    private String Nome;
    private String Categoria;
    private String Marca;
    private double Peso;
    private double Preco;
    private String Descricao;
    
    Produto() {
        Nome = "";
        Categoria = "";
        Marca = "";
        Peso = 0;
        Preco = 0;
        Descricao = "";
    }
    
    public void cadastrar(){
        insertMySQL(Nome, Categoria, Marca, Peso, Preco, Descricao);
    }

    public String getNome() {
        return Nome;
    }

    public void setNome(String Nome) {
        this.Nome = Nome;
    }

    public String getCategoria() {
        return Categoria;
    }

    public void setCategoria(String Categoria) {
        this.Categoria = Categoria;
    }

    public String getMarca() {
        return Marca;
    }

    public void setMarca(String Marca) {
        this.Marca = Marca;
    }

    public double getPeso() {
        return Peso;
    }

    public void setPeso(double Peso) {
        this.Peso = Peso;
    }

    public double getPreco() {
        return Preco;
    }

    public void setPreco(double Preco) {
        this.Preco = Preco;
    }

    public String getDescricao() {
        return Descricao;
    }

    public void setDescricao(String Descricao) {
        this.Descricao = Descricao;
    }
}
