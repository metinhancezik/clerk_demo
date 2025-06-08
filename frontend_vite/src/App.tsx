import { useEffect } from 'react';
import { SignedIn, SignedOut, SignInButton, UserButton, useAuth } from '@clerk/clerk-react';

export default function App() {
    const { getToken, isSignedIn } = useAuth();

    useEffect(() => {
    const sendTokenToBackend = async () => {
      if (isSignedIn) {
        const token = await getToken({ template: "backend" });

        console.log("CLERK TOKEN:", token); // Bunu kontrol et
        if (token) {
          await fetch('https://localhost:5163/api/auth/clerk-token', {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
              'Authorization': `Bearer ${token}`,
            },
          });
        }
      }
    };

    sendTokenToBackend();
  }, [isSignedIn]);
  return (
    <header>
      <SignedOut>
        <SignInButton  />
      </SignedOut>
      <SignedIn>
        <UserButton />
      </SignedIn>
    </header>
  );
}