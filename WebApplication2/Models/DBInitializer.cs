using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WebApplication2.ModelsAction;

namespace WebApplication2.Models
{
    public class DBInitializer : System.Data.Entity.DropCreateDatabaseAlways<DBContext>
    {
        protected override void Seed(DBContext db)
        {
            #region Publisher
            Publisher temp_publisher = new Publisher { PublisherName = "NXB Trẻ", Address = "Dia Chi Cua NXB Tre", PhoneNumber = "012345699", PublisherCode="NXB-Tre", UpdatedDate = DateTime.Today, PublisherId = 2 };
            db.Publishers.Add(temp_publisher);
            Publisher temp_publisher2 = new Publisher { PublisherName = "NXB First News", Address = "Dia Chi Cua NXB First News", PhoneNumber = "012345619", PublisherCode= "NXB-First-News", UpdatedDate = DateTime.Today, PublisherId = 3 };
            db.Publishers.Add(temp_publisher2);
            Publisher default_publisher = new Publisher { PublisherName = "Đang Cập Nhật", Address = "", PhoneNumber = "", PublisherCode = "Dang-Cap-Nhat", UpdatedDate = DateTime.Today, PublisherId = 1};
            db.Publishers.Add(default_publisher);
            Publisher temp_publisher3 = new Publisher { PublisherName = "NXB Văn Học", Address = "Địa Chỉ Của NXB Văn Học", PhoneNumber = "111111111", PublisherCode = "NXB-Van-Hoc", UpdatedDate = DateTime.Today, PublisherId = 4 };
            db.Publishers.Add(temp_publisher3);
            Publisher temp_publisher4 = new Publisher { PublisherName = "NXB Nhã Nam", Address = "Địa Chỉ Của NXB Nhã Nam", PhoneNumber = "221111111", PublisherCode = "NXB-Nha-Nam", UpdatedDate = DateTime.Today, PublisherId = 5 };
            db.Publishers.Add(temp_publisher4);
            Publisher temp_publisher5 = new Publisher { PublisherName = "NXB Kim Đồng", Address = "Địa Chỉ Của NXB Kim Đồng", PhoneNumber = "221111333", PublisherCode = "NXB-Kim-Dong", UpdatedDate = DateTime.Today, PublisherId = 6 };
            db.Publishers.Add(temp_publisher5);
            #endregion

            #region Genre
            Genre genre_1 = new Genre { GenreName = "Tiểu Thuyết", GenreCode="tieu-thuyet" };
            db.Genres.Add(genre_1);
            Genre genre_2 = new Genre { GenreName = "Truyện Tranh", GenreCode="truyen-tranh" };
            db.Genres.Add(genre_2);
            Genre genre_3 = new Genre { GenreName = "Kỹ Năng Sống", GenreCode="ky-nang-song" };
            db.Genres.Add(genre_3);
            Genre genre_4 = new Genre { GenreName = "Hư Cấu", GenreCode = "hu-cau" };
            db.Genres.Add(genre_4);
            Genre genre_5 = new Genre { GenreName = "Khoa Học Viễn Tưởng", GenreCode = "khoa-hoc-vien-tuong" };
            db.Genres.Add(genre_5);
            Genre genre_6 = new Genre { GenreName = "Truyện Hài", GenreCode = "truyen-hai" };
            db.Genres.Add(genre_6);
            Genre genre_7 = new Genre { GenreName = "Lãng Mạn", GenreCode = "lang-man" };
            db.Genres.Add(genre_7);
            #endregion

            #region Author
            Author default_author = new Author { AuthorName = "Đang Cập Nhật", AuthorId = 1 }; //We all know Key Id is auto-generated, but I need to force this Id to be 1 thou
            db.Authors.Add(default_author);
            Author wibu_author = new Author { AuthorName = "Truyện Dịch", AuthorId = 2 };
            db.Authors.Add(wibu_author);
            Author author3 = new Author { AuthorName = "Tô Hoài", AuthorId = 3 };
            db.Authors.Add(author3);
            Author author4 = new Author { AuthorName = "Orson Scott Card"};
            db.Authors.Add(author4);
            Author author5 = new Author { AuthorName = "Cecelia Ahern" };
            db.Authors.Add(author5);
            Author author6 = new Author { AuthorName = "Kosaku Anakubo" };
            db.Authors.Add(author6);
            #endregion

            #region Book
            Book book1 = new Book
            {
                BookName = "Dế Mèn Phiêu Lưu Ký",
                BookCode = "De-Men-Phieu-Luu-Ky",
                Cost = 50000,
                Description = "Một con dế đã từ tay ông thả ra chu du thế giới tìm những điều tốt đẹp cho loài người. " +
                "Và con dế ấy đã mang tên tuổi ông đi cùng trên những chặng đường phiêu lưu đến với cộng đồng những con vật trong văn học thế giới, đến với những xứ sở thiên nhiên và văn hóa của các quốc gia khác. Dế Mèn Tô Hoài đã lại sinh ra Tô Hoài Dế Mèn," +
                " một nhà văn trẻ mãi không già trong văn chương - Nhà phê bình Phạm Xuân Nguyên" + "\nÔng rất hiểu tư duy trẻ thơ, kể với chúng theo cách nghĩ của chúng, lí giải sự vật theo lô gích của trẻ. Hơn thế, với biệt tài miêu tả loài vật, Tô Hoài dựng lên một thế giới gần gũi với trẻ thơ. Khi cần, ông biết đem vào chất du ký khiến cho độc giả nhỏ tuổi vừa hồi hộp theo dõi, vừa thích thú khám phá. - TS Nguyễn Đăng Điệp",
                StorageAmount = 10,
                UpdatedDate = DateTime.Today,
                Authors = author3,
                Publishers = temp_publisher,
                BookId = 1,
                isHidden = 0
            };
            db.Books.Add(book1);
            Book_Genre_Junction book_genre = new Book_Genre_Junction { BookId = 1, GenreId = 4 };
            db.Book_Genre_Junctions.Add(book_genre);
            Book book2 = new Book
            {
                BookName = "Tắt Đèn",
                BookCode = "Tat-Den",
                Cost = 60000,
                Description = "Desc của Sách Tắt Đèn",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = new Author { AuthorName = "Ngô Tất Tố", AuthorId = 4 },
                Publishers = temp_publisher,
                BookId = 2,
                isHidden = 0
            };
            db.Books.Add(book2);
            Book_Genre_Junction book_genre1 = new Book_Genre_Junction { BookId = 2, GenreId = 1 };
            db.Book_Genre_Junctions.Add(book_genre1);
            Book book3 = new Book
            {
                BookName = "Số Đỏ",
                BookCode = "So-Do",
                Cost = 80000,
                Description = "\"Tắt đèn là một cuốn xã hội tiểu thuyết tả cảnh đau khổ của dân quê, của một người đàn bà nhà quê An Nam suốt đời sống trong sự nghèo đói và sự ức hiếp của bọn cường hào và người có thế lực mà lúc nào cũng vẫn hết lòng vì chồng, vì con\". - (Ngô Tất Tố)" +
                "   \n\"Theo tôi tiên tri, thì cuốn Tắt đèn còn phải sống lâu, thọ hơn cả một số văn gia đương kim hôm nay. Chị Dậu đích là tác giả Ngô Tất Tố hoá thân ra mà thôi. Chị Dậu là cái đốm sáng đặc biệt của Tắt đèn. Nếu ví toàn truyện Tắt đèn là một khóm cây, thì chị Dậu là cả gốc cả ngọn cả cành và chính chị Dậu đã nổi gió lên mà rung cho cả cái cây dạ hương Tắt đèn đó lên\". - ( Nguyễn Tuân - 1962 )" +
                "   \n\"Chị Dậu là nhân vật điển hình được người đọc yêu mến. Và người yêu mến chị hơn cả là Ngô Tất Tố. Giữa biết bao tệ nạn và cảnh đời bất công ngang trái ở nông thôn Việt Nam cũ, Ngô Tất Tố đã hết lòng bảo vệ một người phụ nữ là chị Dậu. Nhiều lần chị Dậu bị đẩy vào tình thế hiểm nghèo, rất có thể bị làm nhục nhưng Ngô Tất Tố đã giữ cho chị Dậu được bảo đảm toàn vẹn, giữ trọn phẩm giá, không phải đau đớn, dằn vặt\". - (Hà Minh Đức – 1999)",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = new Author { AuthorName = "Vũ Trọng Phụng", AuthorId = 5 },
                Publishers = temp_publisher,
                BookId = 3,
                isHidden = 0
            };
            db.Books.Add(book3);
            Book_Genre_Junction book_genre2 = new Book_Genre_Junction { BookId = 3, GenreId = 1 };
            db.Book_Genre_Junctions.Add(book_genre2);
            Book book4 = new Book
            {
                BookName = "Bỉ Vỏ",
                BookCode = "Bi-Vo",
                Cost = 50000,
                Description = "Bỉ vỏ của Nguyên Hồng là một trong những tác phẩm xuất sắc nhất của dòng văn học hiện thực phê phán. Qua tác phẩm, những nét tiêu biểu của một lớp người sống dưới đáy xã hội, cuộc sống lầm than, đói khổ của tầng lớp nhân dân lao động và cả bản chất xấu xa thối nát của xã hội thực dân phong kiến đều được khắc họa tài tình dưới ngòi bút tinh tế của tác giả. Bính là cô gái nghèo làng Sòi." +
                "    Vì nhẹ dạ, yêu một gã tham đạc điền và bị hắn bỏ rơi giữa lúc bụng mang dạ chửa. Cô bị cha mẹ hắt hủi, đay nghiến và đứa bé sinh ra phải đem bán đi vì sợ làng bắt vạ. Đau đớn, Bính trốn nhà đi mong tìm được người tình. Sau mấy ngày đêm lang thang đói khát, có lần suýt bị làm nhục ở một vườn hoa, Bính gặp một gã trẻ tuổi nhà giàu. Gã lừa cô vào nhà hãm hiếp và đổ bệnh lậu cho cô." +
                "    Vợ gã bắt gặp, đánh đập Bính tàn nhẫn và lôi cô ra Sở cẩm, vu là gái đĩ. Thế là Bính bị đưa vào nhà “lục xì”, sau đó rơi vào nhà chứa của mụ Tài sế cấu. ",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = new Author { AuthorName = "Nguyên Hồng", AuthorId = 6 },
                Publishers = temp_publisher,
                BookId = 4,
                isHidden = 0
            };
            db.Books.Add(book4);
            Book_Genre_Junction book_genre3 = new Book_Genre_Junction { BookId = 4, GenreId = 1 };
            db.Book_Genre_Junctions.Add(book_genre3);
            Book book5 = new Book
            {
                BookName = "Nise Koi",
                BookCode = "Nise-Koi",
                Cost = 50000,
                Description = "Desc của Sách Nise Koi",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = wibu_author,
                Publishers = temp_publisher,
                BookId = 5,
                isHidden = 0
            };
            db.Books.Add(book5);
            Book_Genre_Junction book_genre4 = new Book_Genre_Junction { BookId = 5, GenreId = 2 };
            db.Book_Genre_Junctions.Add(book_genre4);
            Book_Genre_Junction book_genre4b = new Book_Genre_Junction { BookId = 5, GenreId = 6 };
            db.Book_Genre_Junctions.Add(book_genre4b);
            Book_Genre_Junction book_genre4c = new Book_Genre_Junction { BookId = 5, GenreId = 7 };
            db.Book_Genre_Junctions.Add(book_genre4c);
            db.Books.Add(new Book
            {
                BookName = "Kaguya-Sama: Love Is War",
                BookCode = "Kaguya-Sama-Love-Is-War",
                Cost = 70000,
                Description = "Desc của Sách Kaguya-Sama: Love Is War",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = wibu_author,
                Publishers = temp_publisher,
                BookId = 6,
                isHidden = 0
            });
            Book_Genre_Junction book_genre5 = new Book_Genre_Junction { BookId = 6, GenreId = 2 };
            db.Book_Genre_Junctions.Add(book_genre5);
            Book_Genre_Junction book_genre5b = new Book_Genre_Junction { BookId = 6, GenreId = 6 };
            db.Book_Genre_Junctions.Add(book_genre5b);
            Book_Genre_Junction book_genre5c = new Book_Genre_Junction { BookId = 6, GenreId = 7 };
            db.Book_Genre_Junctions.Add(book_genre5c);
            db.Books.Add(new Book
            {
                BookName = "Puella Magi Madoka Magica: A Different Story",
                BookCode = "Puella-Magi-Madoka-Magica-A-Different-Story",
                Cost = 100000,
                Description = "Desc của Sách Puella Magi Madoka Magica: A Different Story",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = wibu_author,
                Publishers = temp_publisher,
                BookId = 7,
                isHidden = 0
            });
            Book_Genre_Junction book_genre6 = new Book_Genre_Junction { BookId = 7, GenreId = 2 };
            db.Book_Genre_Junctions.Add(book_genre6);
            db.Books.Add(new Book
            {
                BookName = "Dám Nghĩ Lớn",
                BookCode = "Dam-Nghi-Lon",
                Cost = 85000,
                Description = "Desc của Sách Dám Nghĩ Lớn",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = new Author { AuthorName = "David J.Schwartz, PH.D", AuthorId = 7 },
                Publishers = temp_publisher2,
                BookId = 8,
                isHidden = 0
            });
            Book_Genre_Junction book_genre7 = new Book_Genre_Junction { BookId = 8, GenreId = 3 };
            db.Book_Genre_Junctions.Add(book_genre7);
            db.Books.Add(new Book
            {
                BookName = "Ping - Vượt Ao Tù Ra Biển Lớn",
                BookCode = "Ping-Vuot-Ao-Tu-Ra-Bien-Lon",
                Cost = 85000,
                Description = "Desc của Sách Ping - Vượt Ao Tù Ra Biển Lớn",
                StorageAmount = 20,
                UpdatedDate = DateTime.Today,
                Authors = new Author { AuthorName = "Stuart Avery Gold", AuthorId = 8 },
                Publishers = temp_publisher2,
                BookId = 9,
                isHidden = 0
            });
            Book_Genre_Junction book_genre8 = new Book_Genre_Junction { BookId = 9, GenreId = 3 };
            db.Book_Genre_Junctions.Add(book_genre8);
            db.Books.Add(new Book
            {
                BookName = "Trò Chơi Của Ender",
                BookCode = "Tro-Choi-Cua-Ender",
                Cost = 72000,
                Description = "Khi Trái Đất đang đứng trước nguy cơ bị diệt vong bởi sự xâm lược của lũ bọ ngoài hành tinh, hy vọng cứu Trái Đất được dồn toàn bộ vào Ender - một cậu bé thiên tài chưa đầy mười tuổi. " +
                    "\nCùng với những đứa trẻ khác, Ender được đưa lên tàu vũ trụ, bắt đầu quá trình học tập và rèn luyện khắc nghiệt tại Trường Chiến đấu để được tôi luyện thành những người lính thiện nghệ, những chỉ huy tài giỏi, và riêng với Ender - để trở thành vị cứu tinh của toàn nhân loại…" +
                    "\nTrò chơi của Ender không chỉ đơn thuần là câu chuyện về học cách chiến đấu và chiến đấu, học cách chiến thắng và chiến thắng; đây còn là câu chuyện về nỗi cô đơn của những “kẻ hơn người”, về nỗi hoang mang và sự vật lộn tìm hiểu bản chất của chính mình." +
                    "Và hơn hết, nó là câu chuyện về nỗi đau của người chiến thắng, và khát khao của một đứa trẻ bị buộc phải trở thành người hùng cứu thế giới..."+
                    "\n\nXIN HÃY ĐƯA CHÁU VỀ NHÀ.",
                StorageAmount = 15,
                UpdatedDate = DateTime.Today,
                Authors = author4,
                Publishers = temp_publisher3,
                BookId = 10,
                isHidden = 0
            });
            Book_Genre_Junction book_genre9 = new Book_Genre_Junction { BookId = 10, GenreId = 5 };
            db.Book_Genre_Junctions.Add(book_genre9);
            db.Books.Add(new Book
            {
                BookName = "Nơi Cuối Cầu Vồng",
                BookCode = "Noi-Cuoi-Cau-Vong",
                Cost = 63000,
                Description = "Đôi bạn thân thiết Alex và Rosie từ khi bảy tuổi đã bắt đầu trao nhau những bức thư chia sẻ  mọi điều buồn vui trong cuộc sống, từ chuyện con chó cưng của Alex cho tới cô Casey có cái mũi to xấu xí. " +
                "Tuổi thơ láu lỉnh và nghịch ngợm trôi qua, cho tới thuở mười lăm, mười sáu ẩm ương với những xúc cảm hồi hộp và lãng mạn chớm nở… tất cả được gói gọn trong những lá thư dấu kín dưới hộc bàn lớp học. " +
                "Rồi đột ngột, cha Alex được thăng chức, cậu đành phải miễn cưỡng theo gia đình chuyển tới Boston, rời xa Rosie và những ngày tháng ngọt ngào..." +
                "\n\nNơi cuối cầu vồng, cuốn tiểu thuyết viết lên từ những dòng thư, những cuộc chat và những tấm bưu thiếp qua năm tháng, đã tiếp tục đưa Cecelia Ahern trở thành một tác giả bestselling trên khắp thế giới." +
                " Nồng ấm tình bạn, tình yêu, đầy ắp những rung cảm mãnh liệt và những chi tiết bất ngờ, Nơi cuối cầu vồng không chỉ là một cuốn sách hấp dẫn," +
                " mà còn lời nhắn gửi dành riêng cho những tâm hồn tri kỷ phải vượt qua mọi thử thách để đến với nhau, và đủ dũng cảm nói lên những lời chân thật. ",
                StorageAmount = 8,
                UpdatedDate = DateTime.Today,
                Authors = author5,
                Publishers = temp_publisher4,
                BookId = 11,
                isHidden = 0
            });
            Book_Genre_Junction book_genre10 = new Book_Genre_Junction { BookId = 11, GenreId = 2 };
            db.Book_Genre_Junctions.Add(book_genre10);
            Book_Genre_Junction book_genre10b = new Book_Genre_Junction { BookId = 11, GenreId = 6 };
            db.Book_Genre_Junctions.Add(book_genre10b);
            db.Books.Add(new Book
            {
                BookName = "Pokemon Cuộc Phiêu Lưu Của Pippi",
                BookCode = "Pokemon-Cuoc-Phieu-Luu-Cua-Pippi",
                Cost = 20000,
                Description = "Pokémon - Cuộc phiêu lưu của Pippi là một trong những Series về Pokémon đình đám tại Việt Nam. Cách đây gần 2 thập kỉ, cùng với Pokémon Đặc biệt, " +
                "Pippi đã trở thành một trong những tác phẩm gây bão vì mức độ hài hước, lầy lội và siêu vui nhộn tới từ chú Pippi ham ăn ham ngủ." +
                    "Với Pippi,ở đâu có chiến đấu,ở đó còn hi vọng!! Chính sự phấn đấu không ngừng của chú Pokémon này bên cạnh các Pokémon quen thuộc khác như Pikachu,Togepy,Fushigidane... " +
                    "đã đem lại những phút giây thư giãn thực sự với ngay cả những độc giả chưa biết quá nhiều về thế giới Pokémon."+
                    "\n\nBởi vì đơn giản,đó chính là tiếng cười!!",
                StorageAmount = 12,
                UpdatedDate = DateTime.Today,
                Authors = author6,
                Publishers = temp_publisher5,
                BookId = 12,
                isHidden = 0
            });
            Book_Genre_Junction book_genre11 = new Book_Genre_Junction { BookId = 12, GenreId = 7 };
            db.Book_Genre_Junctions.Add(book_genre11);
            #endregion

            #region User
            User temp_user = new User { UserName = "abcde", Name = "Tran A", Password = "abcde", Address = "Đường A Phường B Quận C TpHCM", PhoneNumber = "0123456789", Roles = "Customer" };
            db.Users.Add(temp_user);
            User temp_user2 = new User { UserName = "leg1tUser", Name = "Nguyen D", Password = "abcde", Address = "Đường E Phường F Quận G TpHCM", PhoneNumber = "0987654321", Roles = "Customer" };
            db.Users.Add(temp_user2);
            for (int i = 3; i < 9; i++)
            {
                User temp_user3 = new User { UserName = "user0" + i.ToString(), Name = "User 0" + i.ToString(), Password = "abcde", Address = "Đường D Phường A Quận B TpHCM", PhoneNumber = "0987654321", Roles = "Customer" };
                db.Users.Add(temp_user3);
            }
            User mod_user = new User { UserName = "mod01", Name = "Mod 01", Password = "abcde", Address = "Đường A Phường B Quận C TpHCM", PhoneNumber = "0123456789", Roles = "Mod" };
            db.Users.Add(mod_user);
            User admin_user = new User { UserName = "admin", Name = "Admin", Password = "abcde", Address = "Đường A Phường B Quận C TpHCM", PhoneNumber = "0123456789", Roles = "Admin" };
            db.Users.Add(mod_user);
            #endregion

            #region Cart
            Cart temp_cart = new Cart { Users = temp_user, PurchaseDate = DateTime.Today, ShipmentProgress = 0, ShippingAddress= "Đường A Phường B Quận C TpHCM",
                DeliveryDate = DateTime.Today.AddDays(14), Paid = 0, ShippingCost = 0, Completed = 1 };
            db.Carts.Add(temp_cart);
            db.CartInfos.Add(new CartInfo { BookId = 1 , Amount = 2 , Carts = temp_cart, Books = book1});
            for(int i =1; i < 5; i++)
            {
                DateTime dateTime = DateTime.Now.AddDays(-14 +i*2);
                Cart cart = new Cart { Users = temp_user, PurchaseDate = dateTime, ShipmentProgress = 2, ShippingAddress = "Đường A Phường B Quận C TpHCM",
                    DeliveryDate = dateTime.AddDays(14), Paid = 0, ShippingCost = 0, Completed = 1 };
                db.Carts.Add(cart);
                CartInfo cartInfo = new CartInfo { BookId = i, Amount = i, Carts = cart };
                db.CartInfos.Add(cartInfo);
            }
            Cart cart2 = new Cart
            {
                Users = temp_user2,
                PurchaseDate = DateTime.Today,
                ShipmentProgress = 0,
                ShippingAddress = "Đường A Phường B Quận C TpHCM",
                DeliveryDate = DateTime.Today.AddDays(14),
                Paid = 0,
                TotalCost = book3.Cost*1,
                ShippingCost = 0,
                Completed = 1
            };
            db.Carts.Add(cart2);
            CartInfo cartInfo2 = new CartInfo { BookId = 3, Amount = 1, Carts = cart2 };
            db.CartInfos.Add(cartInfo2);

            Cart cart3 = new Cart
            {
                Users = temp_user2,
                PurchaseDate = DateTime.Today.AddDays(-4),
                ShipmentProgress = 1,
                ShippingAddress = "Đường A Phường B Quận C TpHCM",
                DeliveryDate = DateTime.Today.AddDays(10),
                Paid = 1,
                TotalCost = book3.Cost * 1 + book1.Cost * 2,
                ShippingCost = 0,
                Completed = 1
            };
            db.Carts.Add(cart3);
            CartInfo cartInfo3 = new CartInfo { BookId = 3, Amount = 1, Carts = cart3 };
            db.CartInfos.Add(cartInfo3);
            CartInfo cartInfo3b = new CartInfo { BookId = 1, Amount = 2, Carts = cart3 };
            db.CartInfos.Add(cartInfo3b);

            Cart cart4 = new Cart
            {
                Users = temp_user2,
                PurchaseDate = DateTime.Today.AddDays(-4),
                ShipmentProgress = 2,
                ShippingAddress = "Đường A Phường B Quận C TpHCM",
                DeliveryDate = DateTime.Today.AddDays(10),
                Paid = 1,
                TotalCost = book4.Cost * 1 + book2.Cost * 2,
                ShippingCost = 0,
                Completed = 1
            };
            db.Carts.Add(cart4);
            CartInfo cartInfo4 = new CartInfo { BookId = 4, Amount = 2, Carts = cart4 };
            db.CartInfos.Add(cartInfo4);
            CartInfo cartInfo4b = new CartInfo { BookId = 2, Amount = 3, Carts = cart4 };
            db.CartInfos.Add(cartInfo4b);

            Cart cart5 = new Cart
            {
                Users = temp_user2,
                PurchaseDate = DateTime.Today.AddDays(-4),
                ShipmentProgress = 0,
                ShippingAddress = "Đường A Phường B Quận C TpHCM",
                DeliveryDate = DateTime.Today.AddDays(10),
                Paid = 1,
                TotalCost = book4.Cost *2,
                ShippingCost = 0,
                Completed = 0
            };
            db.Carts.Add(cart5);
            CartInfo cartInfo5 = new CartInfo { BookId = 4, Amount = 2, Carts = cart5 };
            db.CartInfos.Add(cartInfo5);

            Cart cart6 = new Cart
            {
                Users = temp_user2,
                PurchaseDate = DateTime.Today.AddDays(-4),
                ShipmentProgress = 0,
                ShippingAddress = "Đường A Phường B Quận C TpHCM",
                DeliveryDate = DateTime.Today.AddDays(10),
                Paid = 1,
                TotalCost = book2.Cost * 1,
                ShippingCost = 0,
                Completed = -1
            };
            db.Carts.Add(cart6);
            CartInfo cartInfo6 = new CartInfo { BookId = 2, Amount = 1, Carts = cart6 };
            db.CartInfos.Add(cartInfo6);
            #endregion

            #region Promotion
            Promotion promo01 = new Promotion { PromotionId = 1, PromoName = "Kỹ Năng Sống Mùa Covid", PromoDesc = "Giảm giá tất cả sách Kỹ Năng Sống 10 - 20%",
                PromoStartDate = DateTime.ParseExact("01/06/2021","dd/MM/yyyy", CultureInfo.InvariantCulture), PromoEndDate = DateTime.ParseExact("25/12/2021","dd/MM/yyyy", CultureInfo.InvariantCulture), PromoState = 1 };
            db.Promotions.Add(promo01);
            PromotionDetail promoDetail01 = new PromotionDetail { PromotionDetailId = 1, BookId = 8, DiscountedCost = 76500, PromotionId = 1 };
            db.PromotionDetails.Add(promoDetail01);
            PromotionDetail promoDetail02 = new PromotionDetail { PromotionDetailId = 1, BookId = 9, DiscountedCost = 68000, PromotionId = 1 };
            db.PromotionDetails.Add(promoDetail02);

            #endregion
            base.Seed(db);

            //These data are not applied yet, so you can't adjust any data here.
            //To change them, check out the UpdateData method in HomeController (for testing purpose only, ofc)
        }
    }
}