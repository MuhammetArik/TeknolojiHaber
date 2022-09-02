const app = new Vue({
    el: '#app',
    data: {
        kategoriler: [],
        yeniKategori: "",
        duzenlenenKategori:null,
        silinenKategori:null
    },
    methods: {
        
        listele: function () {
            fetch('https://localhost:5001/api/KategoriApi')
                .then(response => response.json())
                .then(data => { console.log(data); this.kategoriler = data; });
        },
        yeniKategoriEkle: function () {
            const kategori = { kategoriAdi: this.yeniKategori };

            fetch('https://localhost:5001/api/KategoriApi', {
                method: 'POST', 
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(kategori),
            })
                .then(response => response.json())
                .then(data => {
                    console.log('Success:', data);
                    this.yeniKategori="";
                    this.listele();
                    TempData["basarilimesaj"]=" Ekleme İşlemi Başarılı.";
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        },
        duzenle: function (kategori) {
            // const kategori = { adi: this.yeniKategori };

            fetch('https://localhost:5001/api/KategoriApi/'+kategori.id, {
                method: 'PUT', 
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(kategori),
            })
                .then(data => {
                    console.log('Success:', data);
                    duzenlenenKategori = null;
                    this.listele();
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        },
        sil: function (kategori) {

            fetch('https://localhost:5001/api/KategoriApi/'+kategori.id, {
                method: 'DELETE', 
                headers: {
                    'Content-Type': 'application/json',
                },
            })
                .then(data => {
                    console.log('Success:', data);
                    this.listele();
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        }

    },
    created: function () {
        this.listele();
    }
})