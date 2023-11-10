using System;
using EFTutorial.Models;
using System.Linq;

namespace EFTutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new BlogContext())
            {
                bool exit = false;
                while (!exit)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Display Blogs");
                    Console.WriteLine("2. Add Blog");
                    Console.WriteLine("3. Display Posts");
                    Console.WriteLine("4. Add Post");
                    Console.WriteLine("5. Exit");
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            var blogs = db.Blogs.ToList();
                            foreach (var b in blogs)
                            {
                                Console.WriteLine($"{b.BlogId}: {b.Name}");
                            }
                            break;

                        case "2":
                            Console.WriteLine("Enter the name of the new Blog:");
                            string name = Console.ReadLine();
                            var blog = new Blog { Name = name };
                            db.Blogs.Add(blog);
                            db.SaveChanges();
                            Console.WriteLine("Blog added.");
                            break;

                        case "3":
                            Console.WriteLine("Available Blogs:");
                            foreach (var b in db.Blogs)
                            {
                                Console.WriteLine($"{b.BlogId}: {b.Name}");
                            }

                            Console.WriteLine("Enter the Blog ID to display its posts:");
                            if (int.TryParse(Console.ReadLine(), out int blogId) && db.Blogs.Any(b => b.BlogId == blogId))
                            {
                                var selectedBlog = db.Blogs
                                    .Where(b => b.BlogId == blogId)
                                    .Select(b => new
                                    {
                                        b.Name,
                                        Posts = b.Posts.Select(p => new { p.Title, p.Content }).ToList()
                                    })
                                    .FirstOrDefault();

                                Console.WriteLine($"Posts for Blog '{selectedBlog.Name}':");
                                foreach (var post in selectedBlog.Posts)
                                {
                                    Console.WriteLine($"- Title: {post.Title}, Content: {post.Content}");
                                }
                                Console.WriteLine($"Total Posts: {selectedBlog.Posts.Count}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Blog ID or Blog does not exist.");
                            }
                            break;

                        case "4":
                            Console.WriteLine("Available Blogs:");
                            foreach (var b in db.Blogs)
                            {
                                Console.WriteLine($"{b.BlogId}: {b.Name}");
                            }

                            Console.WriteLine("Enter the Blog ID you are posting to:");
                            if (int.TryParse(Console.ReadLine(), out int targetBlogId) && db.Blogs.Any(b => b.BlogId == targetBlogId))
                            {
                                Console.WriteLine("Enter the Post title:");
                                string postTitle = Console.ReadLine();
                                Console.WriteLine("Enter the Post content:");
                                string postContent = Console.ReadLine();

                                var newPost = new Post { Title = postTitle, Content = postContent, BlogId = targetBlogId };
                                db.Posts.Add(newPost);
                                db.SaveChanges();
                                Console.WriteLine("Post added.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Blog ID or Blog does not exist.");
                            }
                            break;

                        case "5":
                            exit = true;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }
    }
}
