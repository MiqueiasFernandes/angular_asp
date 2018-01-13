export class User {
    public Email: string;
    public Password: string;
    public FirstName: string;
    public LastName: string;
    public UserName: string;
    public UserPhoto: number[];

   // public file;
    //public funcaoParaEnviar;
    public data: {
        image,
        funcao,
        http,
        snackBar,
        error,
        user: User
    };

    enviar() {
        const reader = new FileReader();
       // const func = this.funcaoParaEnviar;

      const dt = this.data;
      dt.user = this;
        reader.onloadend = function () {

          const  bin2str = function bin2String(array) {
                var result = "";
                for (var i = 0; i < array.length; i++) {
                  result += String.fromCharCode(array[i]);//parseInt(array[i], 2));
                }
                return result;
              }

            const str2bin = function string2Bin(str) {
                var result = [];
                for (var i = 0; i < str.length; i++) {
                  result.push(str.charCodeAt(i));
                  ///.toString(2));
                }
                return result;
              }


            var bytes = []; // char codes
            const str = reader.result;;
            //for (var i = 0; i < str.length; ++i) {
          //      var code = str.charCodeAt(i);
          //      bytes =bytes.concat([code]);
                //bytes = bytes.concat([code & 0xff, code / 256 >>> 0]);
          //  }

            bytes = str2bin(str);

            dt.user.UserPhoto = bytes
            dt.funcao(dt);


            console.log(str.substr(0,100));
            console.log(bin2str(bytes).substr(0,100));


        }
        reader.readAsBinaryString(this.data.image);
    }

}