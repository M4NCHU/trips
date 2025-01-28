/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: ["class"],
  content: [
    './pages/**/*.{ts,tsx}',
    './components/**/*.{ts,tsx}',
    './app/**/*.{ts,tsx}',
    './src/**/*.{ts,tsx}',
  ],
  theme: {
    container: {
      center: true,
      padding: "2rem",
      screens: {
        "5xl": "2000px",
        "4xl": "1800px",
        "3xl": "1600px",
        "2xl": "1400px",
        xl: "1280px",
        lg: "1024px",
        md: "768px",
        sm: "640px",
        xs: "480px",
        "2xs": "360px",
      },
    },
    extend: {
      colors: {
        border: "var(--border)",
        input: "var(--input)",
        ring: "var(--ring)", 
        background: "var(--background)", 
        foreground: "var(--foreground)",
        primary: {
          DEFAULT: "var(--primary)", 
          foreground: "var(--primary-foreground)", 
        },
        secondary: {
          DEFAULT: "var(--secondary)",
          foreground: "var(--secondary-foreground)", 
        },
        destructive: {
          DEFAULT: "var(--destructive)", 
          foreground: "var(--destructive-foreground)", 
        },
        muted: {
          DEFAULT: "var(--muted)", 
          foreground: "var(--muted-foreground)", 
        },
        accent: {
          DEFAULT: "var(--accent)", 
          foreground: "var(--accent-foreground)", 
        },
        popover: {
          DEFAULT: "var(--popover)", 
          foreground: "var(--popover-foreground)", 
        },
        card: {
          DEFAULT: "var(--card)", 
          foreground: "var(--card-foreground)", 
        },

        admin: {
          background: "var(--background)", 
          foreground: "var(--foreground)", 
          primary: {
            DEFAULT: "var(--primary)", 
            foreground: "var(--primary-foreground)", 
          },
          secondary: {
            DEFAULT: "var(--secondary)", 
            foreground: "var(--secondary-foreground)", 
          },
          accent: {
            DEFAULT: "var(--accent)", 
            foreground: "var(--accent-foreground)", 
          },
          muted: {
            DEFAULT: "var(--muted)", 
            foreground: "var(--muted-foreground)", 
          },
        },
        
        centralSection: {
          foreground: "var(--central-section-foreground)", 
          background: "var(--central-section-background)", 
        },
      },
      borderRadius: {
        lg: "var(--radius)",
        md: "calc(var(--radius) - 2px)",
        sm: "calc(var(--radius) - 4px)",
      },
      keyframes: {
        "accordion-down": {
          from: { height: 0 },
          to: { height: "var(--radix-accordion-content-height)" },
        },
        "accordion-up": {
          from: { height: "var(--radix-accordion-content-height)" },
          to: { height: 0 },
        },
      },
      animation: {
        "accordion-down": "accordion-down 0.2s ease-out",
        "accordion-up": "accordion-up 0.2s ease-out",
      },
    },
  },
  plugins: [require("tailwindcss-animate")],
}
