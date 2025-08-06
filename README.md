<p align="center">
  <img src="Assets/GooseberryLogo.png" alt="Gooseberry Logo" width="120" />
</p>

<h1 align="center">Gooseberry Portal</h1>

A super simple WPF desktop app for managing a list of quick-access pages or tools - built because I wanted something lightweight, fast, and focused for my home server setup.

![Main View](screenshots/main-view.png)
![Manager](screenshots/manager.png)
![Add New Page](screenshots/add-new-page.png)

---

## ✨ Features

- ✅ Add, remove, and edit custom "pages" (Web-views)
- ✅ Quick-launch interface
- ✅ Persistent storage (your pages are saved)
- ✅ Minimalist UI with dark styling
- ✅ Packaged as a standalone `.msix` installer

---

## 🧠 Why?

I wanted something like this for my own use - nothing overengineered, just a way to launch the tools and pages I use every day without clutter.

Since it only took an afternoon to make, I figured I'd share it in case it's useful to anyone else.

I might come back to this and improve on it somehow - Let me know if you have any suggestions.

---

# 🧩 How to Install

Because this app isn't signed with a trusted certificate yet, Windows might block it. Here's how to install:

1. Download and run `gooseberry-portal.msix`
   - If it fails, proceed to step 2

2. Download `gooseberry-cert.cer` and double-click it

3. In the wizard:
   - Select **Local Machine**
   - Choose **Place all certificates in the following store**
   - Pick **Trusted People**

4. After installing the certificate, run the `.msix` again.

If you're unsure, you can also enable Developer Mode in Windows settings.

---

## 📌 Notes

- This is a first version - very basic by design
- I'm happy to keep it simple, but if you'd like to see it improved, open an issue or suggest a feature
- Could be fun to keep iterating on it 🙂

---

## 🛠 Tech Stack

- C#
- .NET 8
- WPF

---

## 💬 Feedback Welcome

Got ideas? Suggestions? Want it themed differently or extended with new features?  
Open an issue - Might be fun.
